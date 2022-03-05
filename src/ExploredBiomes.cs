using System.Collections.Generic;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Keeps track of which biomes have been "explored" (science data returned for them).
    /// </summary>
    internal static class ExploredBiomes
    {
        private static readonly Dictionary<string, Dictionary<string, List<string>>> subjectIdsByBodyBiome = new Dictionary<string, Dictionary<string, List<string>>>();

        private static readonly List<ExperimentSituations> relevantSituations = new List<ExperimentSituations>();

        /// <summary>
        /// Add a situation to the list of situations whose experimental results
        /// qualify as "exploring" a biome.
        /// </summary>
        /// <param name="situation"></param>
        public static void AddSituation(ExperimentSituations situation)
        {
            if (!relevantSituations.Contains(situation))
            {
                relevantSituations.Add(situation);
            }
        }

        /// <summary>
        /// Gets the number of biomes on the body that are "explored".
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static int GetExploredBiomeCount(CelestialBody body)
        {
            if (!IsScienceMode) return 0;
            if (body.BiomeCount() == 0) return 0;
            int exploredCount = 0;
            for (int i = 0; i < body.BiomeMap.Attributes.Length; ++i)
            {
                string biome = body.BiomeMap.Attributes[i].name;
                if (IsBiomeExplored(body, biome)) ++exploredCount;
            }
            return exploredCount;
        }

        private static bool IsScienceMode
        {
            get { return ResearchAndDevelopment.Instance != null; }
        }

        /// <summary>
        /// Given a celestial body and a biome, get whether that biome is "explored".
        /// </summary>
        /// <param name="body"></param>
        /// <param name="biome"></param>
        /// <returns></returns>
        private static bool IsBiomeExplored(CelestialBody body, string biome)
        {
            List<string> subjectIds = GetBodyBiomeSubjectIds(body, biome);
            for (int i = 0; i < subjectIds.Count; ++i)
            {
                if (ResearchAndDevelopment.GetSubjectByID(subjectIds[i]) != null) return true;
            }
            return false;
        }

        /// <summary>
        /// Given a celestial body, get a list of all the subject IDs that are relevant
        /// for determining whether a given biome is explored.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        private static List<string> GetBodyBiomeSubjectIds(CelestialBody body, string biome)
        {
            // First, look up by celestial body
            Dictionary<string, List<string>> subjectIdsByBiome;
            if (!subjectIdsByBodyBiome.TryGetValue(body.name, out subjectIdsByBiome))
            {
                subjectIdsByBiome = new Dictionary<string, List<string>>();
                subjectIdsByBodyBiome.Add(body.name, subjectIdsByBiome);
            }

            // Next, look up by biome
            List<string> subjectIds;
            if (!subjectIdsByBiome.TryGetValue(biome, out subjectIds))
            {
                subjectIds = new List<string>();
                subjectIdsByBiome.Add(biome, subjectIds);
                List<string> allExperimentIds = ResearchAndDevelopment.GetExperimentIDs();
                for (int i = 0; i < allExperimentIds.Count; ++i)
                {
                    ScienceExperiment experiment = ResearchAndDevelopment.GetExperiment(allExperimentIds[i]);
                    for (int j = 0; j < relevantSituations.Count; ++j)
                    {
                        ExperimentSituations situation = relevantSituations[j];
                        if (!experiment.BiomeIsRelevantWhile(situation)) continue;
                        subjectIds.Add(SubjectIdOf(experiment, situation, body, biome));
                    }
                }
            }

            return subjectIds;
        }

        private static string SubjectIdOf(
            ScienceExperiment experiment,
            ExperimentSituations situation,
            CelestialBody body,
            string biome)
        {
            if (experiment.BiomeIsRelevantWhile(situation))
            {
                return experiment.id + "@" + body.name + situation.ToString() + biome.Replace(" ", string.Empty);
            }
            else
            {
                return experiment.id + "@" + body.name + situation.ToString();
            }
        }
    }
}
