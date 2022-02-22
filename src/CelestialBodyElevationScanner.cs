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
            Stopwatch timer = new Stopwatch();
            timer.Start();

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
    }
}
