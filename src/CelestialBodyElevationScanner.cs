using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Utility code for scanning a celestial body to find the highest point. Since this
    /// is a very computationally expensive operation, we do the expensive scan only once
    /// and cache the value.
    /// </summary>
    internal static class CelestialBodyElevationScanner
    {
        // Many thanks to Poodmund in the KSP forums, who pointed me at an algorithm
        // implemented in SigmaCartographer that does a good job at this task. The actual
        // code is licensed ARR, so it's not used here, but as a proof of concept it
        // was invaluable.

        private const int INITIAL_SCAN_POINTS = 50_000;
        private const int FILTER_SIZE = 100; // keep this many points out of the initial scan
        private const double SMALLEST_INCREMENT = 0.001; // degrees latitude


        /// <summary>
        /// Given a celestial body, find the highest point on it. If it has been previously
        /// calculated, will just return the previous value. Otherwise, it will do a
        /// computationally expensive scan to locate the highest point.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double GetMaxElevation(CelestialBody body)
        {
            // Skip the whole shebang and just return NaN unless the body actually *has* a surface.
            if (!body.hasSolidSurface) return double.NaN;
            PQS pqs = body.pqsController;
            if (pqs == null) return double.NaN;

            double maxElevation;
            if (!PlanetInfoScenario.TryGetMaxElevation(body, out maxElevation))
            {
                maxElevation = ScanForMaxElevation(body);
                PlanetInfoScenario.SetMaxElevation(body, maxElevation);
            }
            return maxElevation;
        }

        /// <summary>
        /// Do a computationally expensive scan to find the altitude of the highest point
        /// on the specified body.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private static double ScanForMaxElevation(CelestialBody body)
        {
            // Keep track of how long we spend on this.  It's expensive.
            Stopwatch timer = Stopwatch.StartNew();

            // Do a uniform scan of the entire surface of the planet, collecting elevation
            // points in a grid.
            double incrementLatitude = 180.0 / Math.Sqrt((double)INITIAL_SCAN_POINTS);
            int sampleCount = 0;
            List<SurfacePoint> samples = new List<SurfacePoint>((int)(1.05 * INITIAL_SCAN_POINTS));
            samples.Add(SurfacePoint.At(body, 90.0, 0.0));
            for (double latitude = incrementLatitude - 90.0; latitude < 90.0- incrementLatitude; latitude += incrementLatitude)
            {
                double incrementLongitude = incrementLatitude / Math.Cos(latitude * Math.PI / 180.0);
                for (double longitude = -180.0; longitude < 180.0; longitude += incrementLongitude)
                {
                    samples.Add(SurfacePoint.At(body, latitude, longitude));
                    ++sampleCount;
                }
            }
            samples.Add(SurfacePoint.At(body, -90.0, 0.0));

            // Keep the highest N points for further investigation, and drill down.
            SurfacePoint highest = FindHighest(
                body,
                incrementLatitude,
                samples.OrderByDescending(p => p.altitude).Take(FILTER_SIZE),
                ref sampleCount);

            timer.Stop();

            Logging.Log(
                "Scanned highest elevation on " + body.GetDisplayName()
                + " in " + timer.ElapsedMilliseconds + " ms (" + sampleCount + " samples):  "
                + (int)highest.altitude + " m at latitude=" + highest.latitude + ", longitude=" + highest.longitude);

            return highest.altitude;
        }

        /// <summary>
        /// Given a set of candidates, scan around them to find the highest point therein.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="incrementLatitude"></param>
        /// <param name="candidates"></param>
        /// <param name="sampleCount"></param>
        /// <returns></returns>
        private static SurfacePoint FindHighest(CelestialBody body, double incrementLatitude, IEnumerable<SurfacePoint> candidates, ref int sampleCount)
        {
            if (incrementLatitude <= SMALLEST_INCREMENT)
            {
                // This is as fine-grained as we get.  Just pick the best value and return it.
                return candidates.OrderByDescending(p => p.altitude).FirstOrDefault();
            }

            // Do a finer-grained search around each of our candidates.
            int count = candidates.Count();
            //Logging.Log("Recursing " + count + " candidates with increment size " + incrementLatitude + ", sample count " + sampleCount);
            double smallIncrementLatitude = 0.125 * incrementLatitude;
            List<SurfacePoint> highest = new List<SurfacePoint>(500 * count);
            foreach (SurfacePoint candidate in candidates) 
            {
                double incrementLongitude = incrementLatitude / Math.Cos(candidate.latitude * Math.PI / 180.0);
                double smallIncrementLongitude = incrementLongitude * 0.1;
                for (double latitude = candidate.latitude - 0.5 * incrementLatitude; latitude <= candidate.latitude + 0.5 * incrementLatitude; latitude += smallIncrementLatitude)
                {
                    for (double longitude = candidate.longitude - 0.5 * incrementLongitude; longitude <= candidate.longitude + 0.5 * incrementLongitude; longitude += smallIncrementLongitude)
                    {
                        highest.Add(SurfacePoint.At(body, latitude, longitude));
                        ++sampleCount;
                    }
                }
            }

            // Take the best of those and recurse further.
            return FindHighest(body, smallIncrementLatitude, highest.OrderByDescending(p => p.altitude).Take(FILTER_SIZE/2), ref sampleCount);
        }

        /// <summary>
        /// Represents a coordinate on the surface of the planet.
        /// </summary>
        private class SurfacePoint
        {
            public readonly double latitude;
            public readonly double longitude;
            public readonly double altitude;

            private SurfacePoint(double latitude, double longitude, double altitude)
            {
                this.latitude = latitude;
                this.longitude = longitude;
                this.altitude = altitude;
            }

            public static SurfacePoint At(CelestialBody body, double latitude, double longitude)
            {
                return new SurfacePoint(latitude, longitude, body.TerrainAltitude(latitude, longitude, true));
            }
        }

        /// <summary>
        /// Spend up to the specified number of milliseconds pre-calculating the highest point
        /// on as many planets as possible, in priority order (starting with the homeworld).
        /// </summary>
        /// <param name="durationMillis"></param>
        public static void Precalculate(double durationMillis)
        {
            // Get all the bodies in the solar system, and sort them in priority order.
            List<CelestialBody> allBodies = new List<CelestialBody>(FlightGlobals.Bodies.Where(b => b.hasSolidSurface));
            allBodies.Sort(PlanetComparer.Instance);
            Logging.Log("Pre-calculating maximum elevations for up to " + durationMillis + " ms (" + allBodies.Count + " total bodies)");

            // Start pre-calculating.  We just call GetMaxElevation on every single one,
            // starting with the beginning.  Any that have previously been calculated
            // (either on a prior pre-calculation run, or during gameplay) will simply
            // immediately return the previously calculated value, so this way we know
            // we're spending the full time allotted in calculating bodies that weren't
            // previously done.
            Stopwatch timer = Stopwatch.StartNew();
            int done = 0;
            for (int i = 0; i < allBodies.Count; i++)
            {
                GetMaxElevation(allBodies[i]);
                done++;
                if (timer.ElapsedMilliseconds > durationMillis) break;
            }
            int percentDone = (int)(100.0 * (double)done / (double)allBodies.Count);
            Logging.Log("Elapsed time " + timer.ElapsedMilliseconds + ", " + percentDone + "% of bodies have been calculated.");
        }

        //========================== PLANET SORTING ====================================
        // The various functions below determine in what order we precalculate expensive-to-compute
        // data for the planets in the system. The rationale is that we want to pick the "user is
        // most likely to look at this" bodies first.  So we start with the homeworld, then
        // any moons it has, etc.

        /// <summary>
        /// Prioritize homeworld over others.
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int HomeworldComparison(CelestialBody b1, CelestialBody b2) => b2.isHomeWorld.CompareTo(b1.isHomeWorld);

        /// <summary>
        /// Prioritize homeworld moons over others.
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int HomeworldMoonComparison(CelestialBody b1, CelestialBody b2) => HomeworldComparison(b1.referenceBody, b2.referenceBody);

        /// <summary>
        /// Prioritize homeworld siblings (planets orbiting the same primary as the homeworld) over others.
        /// Only really matters for modded solar systems, where the homeworld might be a moon of some planet.
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int SiblingComparison(CelestialBody b1, CelestialBody b2) => b2.IsHomeworldSibling().CompareTo(b1.IsHomeworldSibling());

        /// <summary>
        /// Prioritize planets over moons.
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int HierarchyComparison(CelestialBody b1, CelestialBody b2) => b1.HierarchyLevel().CompareTo(b2.HierarchyLevel());

        /// <summary>
        /// Prioritize moons of inner planets over moons of outer ones
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int MoonParentComparison(CelestialBody b1, CelestialBody b2) => SmaComparison(b1.referenceBody, b2.referenceBody);

        /// <summary>
        /// Prioritize smaller orbits over bigger.
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int SmaComparison(CelestialBody b1, CelestialBody b2) => b1.SMA().CompareTo(b2.SMA());

        /// <summary>
        /// Alphabetical by name.
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        private static int NameComparison(CelestialBody b1, CelestialBody b2) => b1.name.CompareTo(b2.name);


        /// <summary>
        /// Used for sorting planets into priority order for pre-caching calculations.
        /// </summary>
        private class PlanetComparer : IComparer<CelestialBody>
        {
            public delegate int Comparison(CelestialBody body1, CelestialBody body2);

            public static PlanetComparer Instance = new PlanetComparer();

            // Comparisons to use for prioritizing bodies to pre-cache, in descending
            // order of importance.
            private static readonly Comparison[] comparisons = {
                HomeworldComparison,
                HomeworldMoonComparison,
                SiblingComparison,
                HierarchyComparison,
                MoonParentComparison,
                SmaComparison,
                NameComparison
            };

            public int Compare(CelestialBody body1, CelestialBody body2)
            {
                for (int i = 0; i < comparisons.Length; i++)
                {
                    int comparison = comparisons[i].Invoke(body1, body2);
                    if (comparison != 0) return comparison;
                }
                return 0;
            }
        }
    }
}
