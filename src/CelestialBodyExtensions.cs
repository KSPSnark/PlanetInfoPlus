using System;

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
    }
}
