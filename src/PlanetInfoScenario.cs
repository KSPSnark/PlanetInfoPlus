using System.Collections.Generic;
using System.IO;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Stores persisted data in the .sfs file. This is where we remember information
    /// to be persisted across play sessions.
    /// </summary>
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.TRACKSTATION, GameScenes.FLIGHT)]
    class PlanetInfoScenario : ScenarioModule
    {
        private const string TIMESTAMP = "PlanetInfoPlusTimestamp";
        private const string ELEVATION_PREFIX = "elevation:";

        // The timestasmp of the assembly where this lives (i.e. PlanetInfoPlus.DLL). We
        // store all cached information associated with this timestamp, so that we'll know
        // to ignore old/obsolete cached data (which might be wrong) when the mod is updated.
        private static readonly long timestamp = File.GetLastWriteTimeUtc(typeof(PlanetInfoScenario).Assembly.Location).Ticks;

        private static readonly Dictionary<string, double> maxPlanetElevations = new Dictionary<string, double>();

        /// <summary>
        /// Try to get the max elevation of the specified planet. Returns true if found, false if not.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="maxElevation"></param>
        /// <returns></returns>
        public static bool TryGetMaxElevation(CelestialBody body, out double maxElevation)
        {
            return maxPlanetElevations.TryGetValue(body.name, out maxElevation);
        }

        /// <summary>
        /// Set the max elevation of the specified planet.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="maxElevation"></param>
        public static void SetMaxElevation(CelestialBody body, double maxElevation)
        {
            maxPlanetElevations[body.name] = maxElevation;
        }

        /// <summary>
        /// Here when the scenario is loading. It's called after the other stock
        /// info is loaded.
        /// </summary>
        /// <param name="node"></param>
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
            maxPlanetElevations.Clear();

            // Check timestamp. If our previous cached information has the wrong timestamp
            // on it, then clear the cache.
            long previousTimestamp = LoadTimestamp(node);
            if (previousTimestamp != timestamp)
            {
                if (previousTimestamp == 0)
                {
                    Logging.Log("No previous cached data found");
                }
                else
                {
                    Logging.Log("Ignoring cached data with timestamp " + previousTimestamp + " (current = " + timestamp + ")");
                }
                return;
            }

            Logging.Log("Read cache timestamp: " + timestamp);

            // Timestamp is good. Load the info.
            for (int i = 0; i < node.values.Count; i++)
            {
                ConfigNode.Value value = node.values[i];
                if (!value.name.StartsWith(ELEVATION_PREFIX)) continue;
                string planetName = value.name.Substring(ELEVATION_PREFIX.Length);
                double elevation = double.Parse(value.value);
                Logging.Log("Read max elevation of " + planetName + ": " + elevation);
                maxPlanetElevations.Add(planetName, elevation);
            }
        }

        /// <summary>
        /// Here when the scenario is saving. It's called after the other stock info is saved.
        /// </summary>
        /// <param name="node"></param>
        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
            if (maxPlanetElevations.Count == 0) return;

            Logging.Log("Writing cache timestamp: " + timestamp);
            node.AddValue(TIMESTAMP, timestamp);

            foreach (KeyValuePair<string, double> maxElevation in maxPlanetElevations)
            {
                Logging.Log("Write max elevation of " + maxElevation.Key + ": " + maxElevation.Value);
                node.AddValue(ELEVATION_PREFIX + maxElevation.Key, maxElevation.Value);
            }
        }

        private static long LoadTimestamp(ConfigNode node)
        {
            for (int i = 0; i < node.values.Count; i++)
            {
                if (TIMESTAMP.Equals(node.values[i].name))
                {
                    return long.Parse(node.values[i].value);
                }
            }
            // not found, e.g. may never have been run before
            return 0;
        }
    }
}
