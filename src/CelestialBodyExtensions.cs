using KSPAchievements;
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
            return CelestialBodyElevationScanner.GetMaxElevation(body).altitude;
        }

        public static double SMA(this CelestialBody body)
        {
            return (body.orbit == null) ? 0.0 : body.orbit.semiMajorAxis;
        }

        /// <summary>
        /// Returns true if this is the body that the homeworld orbits.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool IsHomeworldParent(this CelestialBody body)
        {
            return body.flightGlobalsIndex == FlightGlobals.GetHomeBody().referenceBody.flightGlobalsIndex;
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

            return body.referenceBody.flightGlobalsIndex == FlightGlobals.GetHomeBody().referenceBody.flightGlobalsIndex;
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

        /// <summary>
        /// Gets the number of biomes for the body. (Does not include "mini-biomes"
        /// such as the ones around KSC.)
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static int BiomeCount(this CelestialBody body)
        {
            if (body.BiomeMap == null) return 0;
            if (body.BiomeMap.Attributes == null) return 0;
            return body.BiomeMap.Attributes.Length;
        }

        /// <summary>
        /// Describes the most "advanced" exploration of the body yet done.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string ExplorationDescription(this CelestialBody body)
        {
            ExplorationProgress progress = ExplorationProgress.For(body);
            return (progress == null) ? Strings.NONE : progress.Description;
        }

        /// <summary>
        /// Returns whether the player has visited this body or not.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static bool IsExplored(this CelestialBody body)
        {
            if (body.isHomeWorld) return true; // by definition ;-)
            CelestialBodySubtree progress = body.progressTree;
            return IsComplete(progress.flyBy)
                || IsComplete(progress.orbit)
                || IsComplete(progress.suborbit)
                || IsComplete(progress.flight)
                || IsComplete(progress.escape);
        }

        private static bool IsComplete(ProgressNode node)
        {
            return (node != null) && node.IsComplete;
        }

        /// <summary>
        /// Gets how many biomes have been explored (returned science data).
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static int ExploredBiomeCount(this CelestialBody body)
        {
            return body.IsExplored() ? ExploredBiomes.GetExploredBiomeCount(body) : 0;
        }
    }
}
