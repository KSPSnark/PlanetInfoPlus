using System;
using System.Collections.Generic;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Extension methods for CelestialBody.
    /// </summary>
    internal static class CelestialBodyExtensions
    {
        /// <summary>
        /// Gets whether the body has an orbit.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool HasOrbit(this CelestialBody body)
        {
            if (body == null) return false;
            if (body.referenceBody == null) return false;
            if (body.orbit == null) return false;

            return true;
        }

        /// <summary>
        /// Get the height above surface level of a synchronous orbit.  Returns NaN if not possible.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double GetSynchronousAltitude(this CelestialBody body)
        {
            double param = body.rotationPeriod / (2.0 * Math.PI);
            double radius = Math.Pow(body.gravParameter * param * param, 1.0 / 3.0);
            if (radius > body.sphereOfInfluence) return double.NaN;
            return radius - body.Radius;
        }

        /// <summary>
        /// Get the elevation of the highest peak on the planet. Returns NaN if the
        /// planet has no surface.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static double MaxElevation(this CelestialBody body)
        {
            return CelestialBodyElevationScanner.GetMaxElevation(body);
        }

        public static double SMA(this CelestialBody body)
        {
            return (body.orbit == null) ? 0.0 : body.orbit.semiMajorAxis;
        }

        /// <summary>
        /// Returns true if this body has the same parent as the homeworld.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool IsHomeworldSibling(this CelestialBody body)
        {
            if (body.isHomeWorld) return false;
            if (body.flightGlobalsIndex == body.referenceBody.flightGlobalsIndex) return false; // it's the sun

            return body.referenceBody.flightGlobalsIndex == FlightGlobals.GetHomeBodyIndex();
        }

        /// <summary>
        /// Returns the orbital hierarchy level, e.g. 0 for the sun,
        /// 1 for planets, 2 for their moons, etc.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static int HierarchyLevel(this CelestialBody body)
        {
            int level = 0;
            while (body.flightGlobalsIndex != body.referenceBody.flightGlobalsIndex)
            {
                level++;
                if (level > 100)
                {
                    Logging.Error("ERROR! Hierarchy overflow for " + body.GetDisplayName() + ", parent " + body.referenceBody.GetDisplayName());
                    break;
                }
                body = body.referenceBody;
            }
            return level;
        }
    }
}
