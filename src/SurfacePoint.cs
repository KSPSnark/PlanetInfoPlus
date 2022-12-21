using System;
using System.Collections.Generic;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Represents a coordinate on the surface of the planet.
    /// </summary>
    internal class SurfacePoint
    {
        public static readonly Dictionary<string, SurfacePoint> maxPlanetElevations
            = new Dictionary<string, SurfacePoint>();

        public readonly double latitude;
        public readonly double longitude;
        public readonly double altitude;

        private SurfacePoint(double latitude, double longitude, double altitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
        }

        /// <summary>
        /// Gets the surface point at the specified coordinates.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="latitude">Latitude, in degrees</param>
        /// <param name="longitude">Longitude, in degrees</param>
        /// <returns></returns>
        public static SurfacePoint At(CelestialBody body, double latitude, double longitude)
        {
            return new SurfacePoint(latitude, longitude, body.TerrainAltitude(latitude, longitude, true));
        }

        /// <summary>
        /// Parses a SurfacePoint from a string representation thereof, in the format which
        /// ToString() produces.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static SurfacePoint Parse(string text)
        {
            string[] parts = text.Split(',');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid surface point format (arg count): " + text);
            }
            double altitude = double.Parse(parts[0].Trim());
            double latitude = double.Parse(parts[1].Trim());
            double longitude = double.Parse(parts[2].Trim());
            return new SurfacePoint(latitude, longitude, altitude);
        }

        /// <summary>
        /// String representation of a SurfacePoint. Used for persisting to save files.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return altitude.ToString() + ", " + latitude.ToString() + ", " + longitude.ToString();
        }
    }
}
