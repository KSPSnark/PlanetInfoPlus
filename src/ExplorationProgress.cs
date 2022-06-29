using KSPAchievements;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Tracks the most "advanced" level of exploration attained for a given
    /// celestial body.
    /// </summary>
    internal class ExplorationProgress
    {
        public delegate bool Achieved(CelestialBodySubtree progress);

        private readonly Achieved achieved;
        private readonly string description;

        private static readonly ExplorationProgress plantedFlag      = Of(a => IsComplete(a.flagPlant),        Strings.PROGRESS_PLANTED_FLAG);
        private static readonly ExplorationProgress landingCrewed    = Of(a => IsCompleteManned(a.landing),    Strings.PROGRESS_LANDING_CREWED);
        private static readonly ExplorationProgress splashdownCrewed = Of(a => IsCompleteManned(a.splashdown), Strings.PROGRESS_SPLASHDOWN_CREWED);
        private static readonly ExplorationProgress landing          = Of(a => IsComplete(a.landing),          Strings.PROGRESS_LANDING);
        private static readonly ExplorationProgress splashdown       = Of(a => IsComplete(a.splashdown),       Strings.PROGRESS_SPLASHDOWN);
        private static readonly ExplorationProgress orbitCrewed      = Of(a => IsCompleteManned(a.orbit),      Strings.PROGRESS_ORBIT_CREWED);
        private static readonly ExplorationProgress orbit            = Of(a => IsComplete(a.orbit),            Strings.PROGRESS_ORBIT);
        private static readonly ExplorationProgress flybyCrewed      = Of(a => IsCompleteManned(a.flyBy),      Strings.PROGRESS_FLYBY_CREWED);
        private static readonly ExplorationProgress flyby            = Of(a => IsComplete(a.flyBy),            Strings.PROGRESS_FLYBY);
        private static readonly ExplorationProgress suborbitCrewed   = Of(a => IsCompleteManned(a.suborbit),   Strings.PROGRESS_SUBORBIT_CREWED);
        private static readonly ExplorationProgress suborbit         = Of(a => IsComplete(a.suborbit),         Strings.PROGRESS_SUBORBIT);
        private static readonly ExplorationProgress flightCrewed     = Of(a => IsCompleteManned(a.flight),     Strings.PROGRESS_FLIGHT_CREWED);
        private static readonly ExplorationProgress flight           = Of(a => IsComplete(a.flight),           Strings.PROGRESS_FLIGHT);

        /// <summary>
        /// In descending order, the most impressive accomplishments for the homeworld.
        /// </summary>
        private static readonly ExplorationProgress[] homeworldProgress = new ExplorationProgress[]
        {
            orbitCrewed,
            orbit,
            suborbitCrewed,
            suborbit,
            landingCrewed,
            splashdownCrewed,
            landing,
            splashdown,
            flightCrewed,
            flight,
            plantedFlag
        };

        /// <summary>
        /// In descending order, the most impressive accomplishments for
        /// any other world than the homeworld.
        /// </summary>
        private static readonly ExplorationProgress[] generalProgress = new ExplorationProgress[]
        {
            plantedFlag,
            landingCrewed,
            splashdownCrewed,
            landing,
            splashdown,
            flightCrewed,
            flight,
            orbitCrewed,
            orbit,
            flybyCrewed,
            flyby,
        };

        /// <summary>
        /// Given a celestial body, find the most advanced exploration done.
        /// If the body has never been explored, returns null.
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ExplorationProgress For(CelestialBody body)
        {
            ExplorationProgress[] candidates = body.isHomeWorld ? homeworldProgress : generalProgress;
            CelestialBodySubtree progress = body.progressTree;
            if (progress == null) return null;
            for (int i = 0; i < candidates.Length; i++)
            {
                if (candidates[i].achieved.Invoke(progress)) return candidates[i]; ;
            }
            return null;
        }

        public string Description { get { return description; } }

        private ExplorationProgress(Achieved achieved, string description)
        {
            this.achieved = achieved;
            this.description = description;
        }

        private static ExplorationProgress Of(Achieved achieved, string description) => new ExplorationProgress(achieved, description);

        private static bool IsComplete(ProgressNode node) => (node != null) && node.IsComplete;

        private static bool IsCompleteManned(ProgressNode node) => (node != null) && node.IsCompleteManned;
    }
}
