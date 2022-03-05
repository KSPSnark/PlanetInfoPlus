using System;
using System.Collections.Generic;
using System.IO;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Stores persisted data in the .sfs file. This is where we remember information
    /// to be persisted across play sessions.
    /// </summary>
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.SPACECENTER)]
    class PlanetInfoScenario : ScenarioModule
    {
        private const string TIMESTAMP = "PlanetInfoPlusTimestamp";
        private const string ELEVATION_PREFIX = "elevation:";

        // The timestasmp of the assembly where this lives (i.e. PlanetInfoPlus.DLL). We
        // store all cached information associated with this timestamp, so that we'll know
        // to ignore old/obsolete cached data (which might be wrong) when the mod is updated.
        private static readonly long timestamp = File.GetLastWriteTimeUtc(typeof(PlanetInfoScenario).Assembly.Location).Ticks;

        /// <summary>
        /// Here when the scenario is loading. It's called after the other stock
        /// info is loaded.
        /// </summary>
        /// <param name="node"></param>
        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);

            // There's only one time that we ever want to *actually* load the max elevation
            // data out of the scenario from the game's savefile, and that's on first
            // run when loading up the game. If we have any entries already in memory,
            // it means some combination of "we already loaded them from the savefile before"
            // and/or "we've been calculating some additional values ourselves". So, if we
            // already have anything in RAM, don't read anything from the savefile and just
            // skip the rest of this processing.
            if (SurfacePoint.maxPlanetElevations.Count > 0) return;

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
                SurfacePoint point;
                try
                {
                    point = SurfacePoint.Parse(value.value);
                }
                catch (Exception ex)
                {
                    Logging.Warn("Invalid surface point format found for " + planetName + ", ignoring: " + ex.Message);
                    continue;
                }
                Logging.Log("Read max elevation of " + planetName + ": " + point.altitude + " m at lat=" + point.latitude + ", lon=" + point.longitude);
                SurfacePoint.maxPlanetElevations.Add(planetName, point);
            }
        }

        /// <summary>
        /// Here when the scenario is saving. It's called after the other stock info is saved.
        /// </summary>
        /// <param name="node"></param>
        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);

            if (SurfacePoint.maxPlanetElevations.Count == 0) return;

            Logging.Log("Writing cache timestamp: " + timestamp);
            node.AddValue(TIMESTAMP, timestamp);

            List<string> planets = new List<string>(SurfacePoint.maxPlanetElevations.Keys);
            planets.Sort();
            foreach (string planet in planets)
            {
                SurfacePoint maxElevation = SurfacePoint.maxPlanetElevations[planet];
                Logging.Log("Write max elevation of " + planet + ": " + maxElevation.altitude
                    + " m at lat=" + maxElevation.latitude + ", lon=" + maxElevation.longitude);
                node.AddValue(ELEVATION_PREFIX + planet, maxElevation.ToString());
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
