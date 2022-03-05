using KSP.Localization;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Various localization tags defined in KSP.
    /// </summary>
    internal static class Strings
    {
        // Parameter names (physical)
        public static readonly string PHYSICAL_CHARACTERISTICS_HEADER = Localizer.Format(Tags.PHYSICAL_CHARACTERISTICS_HEADER);
        public static readonly string EQ_RADIUS = Localizer.Format(Tags.EQ_RADIUS);
        public static readonly string AREA = Localizer.Format(Tags.AREA);
        public static readonly string MASS = Localizer.Format(Tags.MASS);
        public static readonly string GRAV_PARAMETER = Localizer.Format(Tags.GRAV_PARAMETER);
        public static readonly string GRAVITY_ASL = Localizer.Format(Tags.GRAVITY_ASL);
        public static readonly string ESCAPE_VELOCITY = Localizer.Format(Tags.ESCAPE_VELOCITY);
        public static readonly string ROTATION_PERIOD = Localizer.Format(Tags.ROTATION_PERIOD);
        public static readonly string SOI = Localizer.Format(Tags.SOI);

        // Parameter names (atmospheric)
        public static readonly string ATMOSPHERE_CHARACTERISTICS_HEADER_LONG = Localizer.Format(Tags.ATMOSPHERE_CHARACTERISTICS_HEADER_LONG);
        public static readonly string ATMOSPHERE_CHARACTERISTICS_HEADER = InfoColors.Colorize(
            Localizer.Format(Tags.ATMOSPHERE_CHARACTERISTICS_HEADER),
            InfoColors.ColorOf(ATMOSPHERE_CHARACTERISTICS_HEADER_LONG));
        public static readonly string ATMOSPHERE_PRESENT = Localizer.Format(Tags.ATMOSPHERE_PRESENT);
        public static readonly string ATMOSPHERE_HEIGHT = Localizer.Format(Tags.ATMOSPHERE_HEIGHT);
        public static readonly string ATMOSPHERE_PRESSURE = Localizer.Format(Tags.ATMOSPHERE_PRESSURE);
        public static readonly string ATMOSPHERE_ASL_TEMP = Localizer.Format(Tags.ATMOSPHERE_ASL_TEMP);

        // Units
        public static readonly string M = Localizer.Format("#autoLOC_7001411");
        public static readonly string KM = Localizer.Format("#autoLOC_7001405");
        public static readonly string M2 = Localizer.Format("#autoLOC_7001402");
        public static readonly string KG = Localizer.Format("#autoLOC_7001403");
        public static readonly string M3_PER_S2 = Localizer.Format("#autoLOC_7001404");
        public static readonly string M_PER_S = Localizer.Format("#autoLOC_7001415");
        public static readonly string K = Localizer.Format("#autoLOC_7001406");
        public static readonly string GRAVITIES = Localizer.Format("#autoLOC_7001413");
        public static readonly string ATMOSPHERES = Localizer.Format("#autoLOC_7001419");

        // Misc
        public static readonly string YES = Localizer.Format("#autoLOC_439855");
        public static readonly string NO = Localizer.Format("#autoLOC_439856");
        public static readonly string ALL = Localizer.Format("#autoLOC_900712");
        public static readonly string NONE = Localizer.Format("#autoLOC_6003000");
        public static readonly string NOT_APPLICABLE = Localizer.Format("#autoLOC_258912");

        // PlanetInfoPlus terms
        public static readonly string MAX_ELEVATION = Localizer.Format(Tags.MAX_ELEVATION);
        public static readonly string SYNCHRONOUS_ALTITUDE = Localizer.Format(Tags.SYNCHRONOUS_ALTITUDE);
        public static readonly string ORBITAL_PERIOD = Localizer.Format(Tags.ORBITAL_PERIOD);
        public static readonly string LOCKED_ROTATION = Localizer.Format("#PlanetInfoPlus_lockedRotation");
        public static readonly string RETROGRADE_ROTATION = Localizer.Format("#PlanetInfoPlus_retrogradeRotation");
        public static readonly string LOCKED = Localizer.Format("#PlanetInfoPlus_locked");
        public static readonly string OXYGENATED = Localizer.Format("#PlanetInfoPlus_oxygenated");
        public static readonly string GAMEPLAY_CHARACTERISTICS_HEADER = Localizer.Format("#PlanetInfoPlus_gameplayHeader");
        public static readonly string UPPER_ATMOSPHERE_HEIGHT = Localizer.Format(Tags.UPPER_ATMOSPHERE_HEIGHT);
        public static readonly string NEAR_SPACE_HEIGHT = Localizer.Format(Tags.NEAR_SPACE_HEIGHT);
        public static readonly string BIOME_COUNT = Localizer.Format(Tags.BIOME_COUNT);
        public static readonly string EXPLORED_BIOME_COUNT = Localizer.Format(Tags.EXPLORED_BIOME_COUNT);
        public static readonly string EXPLORATION = Localizer.Format(Tags.EXPLORATION);
        public static readonly string PROGRESS_PLANTED_FLAG = Localizer.Format("#PlanetInfoPlus_plantedFlag");
        public static readonly string PROGRESS_LANDING_CREWED = Localizer.Format("#PlanetInfoPlus_landingCrewed");
        public static readonly string PROGRESS_SPLASHDOWN_CREWED = Localizer.Format("#PlanetInfoPlus_splashDownCrewed");
        public static readonly string PROGRESS_LANDING = Localizer.Format("#PlanetInfoPlus_landing");
        public static readonly string PROGRESS_SPLASHDOWN = Localizer.Format("#PlanetInfoPlus_splashDown");
        public static readonly string PROGRESS_ORBIT_CREWED = Localizer.Format("#PlanetInfoPlus_orbitCrewed");
        public static readonly string PROGRESS_ORBIT = Localizer.Format("#PlanetInfoPlus_orbit");
        public static readonly string PROGRESS_FLYBY_CREWED = Localizer.Format("#PlanetInfoPlus_flybyCrewed");
        public static readonly string PROGRESS_FLYBY = Localizer.Format("#PlanetInfoPlus_flyby");
        public static readonly string PROGRESS_SUBORBIT_CREWED = Localizer.Format("#PlanetInfoPlus_suborbitCrewed");
        public static readonly string PROGRESS_SUBORBIT = Localizer.Format("#PlanetInfoPlus_suborbit");
        public static readonly string PROGRESS_FLIGHT_CREWED = Localizer.Format("#PlanetInfoPlus_flightCrewed");
        public static readonly string PROGRESS_FLIGHT = Localizer.Format("#PlanetInfoPlus_flightCrewed");

        /// <summary>
        /// The raw localizer tags.
        /// </summary>
        public static class Tags
        {
            // Parameter names (physical)
            public const string PHYSICAL_CHARACTERISTICS_HEADER = "#autoLOC_462403"; // is colorized
            public const string EQ_RADIUS = "#autoLOC_462417";
            public const string AREA = "#autoLOC_462420";
            public const string MASS = "#autoLOC_462423";
            public const string GRAV_PARAMETER = "#autoLOC_462426";
            public const string GRAVITY_ASL = "#autoLOC_462429";
            public const string ESCAPE_VELOCITY = "#autoLOC_462432";
            public const string ROTATION_PERIOD = "#autoLOC_462435";
            public const string SOI = "#autoLOC_462438";

            // Parameter names (atmospheric
            public static readonly string ATMOSPHERE_CHARACTERISTICS_HEADER_LONG = "#autoLOC_462406"; // is colorized
            public static readonly string ATMOSPHERE_CHARACTERISTICS_HEADER = "#autoLOC_8003223";
            // A note about ATMOSPHERE_CHARACTERISTICS_HEADER, above. The actual tag that
            // stock KSP uses for this is the long one, which in English is rendered as
            // "Atmospheric Characteristics:".  However, that's long enough that it word-wraps
            // in the header in the window, which wastes vertical real estate. Therefore, I've
            // swapped it for this value, which is rendered as "Atmosphere:" and fits better.
            // If I ever figure out how to widen the app window a bit, I may switch it back again.
            public const string ATMOSPHERE_PRESENT = "#autoLOC_462448";
            public const string ATMOSPHERE_HEIGHT = "#autoLOC_462453";
            public const string ATMOSPHERE_PRESSURE = "#autoLOC_462456";
            public const string ATMOSPHERE_ASL_TEMP = "#autoLOC_462459";

            // PlanetInfoPlus tags
            public const string PHYSICAL_SETTINGS_LABEL = "#PlanetInfoPlus_physicalSettingsLabel";
            public const string ATMOSPHERIC_SETTINGS_LABEL = "#PlanetInfoPlus_atmosphericSettingsLabel";
            public const string GAMEPLAY_SETTINGS_LABEL = "#PlanetInfoPlus_gameplaySettingsLabel";
            public const string MAX_ELEVATION = "#PlanetInfoPlus_maxElevation";
            public const string SYNCHRONOUS_ALTITUDE = "#PlanetInfoPlus_synchronousAltitude";
            public const string ORBITAL_PERIOD = "#PlanetInfoPlus_orbitalPeriod";
            public const string UPPER_ATMOSPHERE_HEIGHT = "#PlanetInfoPlus_upperAtmosphereHeight";
            public const string NEAR_SPACE_HEIGHT = "#PlanetInfoPlus_nearSpaceHeight";
            public const string BIOME_COUNT = "#PlanetInfoPlus_biomeCount";
            public const string EXPLORED_BIOME_COUNT = "#PlanetInfoPlus_exploredBiomeCount";
            public const string EXPLORATION = "#PlanetInfoPlus_exploration";
        }
    }
}
