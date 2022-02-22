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
        public static readonly string ATMOSPHERE_CHARACTERISTICS_HEADER = Localizer.Format(Tags.ATMOSPHERE_CHARACTERISTICS_HEADER);
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

        // PlanetInfoPlus terms
        public static readonly string MAX_ELEVATION = Localizer.Format(Tags.MAX_ELEVATION);
        public static readonly string SYNCHRONOUS_ALTITUDE = Localizer.Format(Tags.SYNCHRONOUS_ALTITUDE);
        public static readonly string ORBITAL_PERIOD = Localizer.Format(Tags.ORBITAL_PERIOD);
        public static readonly string LOCKED_ROTATION = Localizer.Format("#PlanetInfoPlus_lockedRotation");
        public static readonly string RETROGRADE_ROTATION = Localizer.Format("#PlanetInfoPlus_retrogradeRotation");
        public static readonly string LOCKED = Localizer.Format("#PlanetInfoPlus_locked");
        public static readonly string NOT_APPLICABLE = Localizer.Format("#PlanetInfoPlus_notApplicable");
        public static readonly string OXYGENATED = Localizer.Format("#PlanetInfoPlus_oxygenated");

        /// <summary>
        /// The raw localizer tags.
        /// </summary>
        public static class Tags
        {
            // Parameter names (physical)
            public const string PHYSICAL_CHARACTERISTICS_HEADER = "#autoLOC_462403";
            public const string EQ_RADIUS = "#autoLOC_462417";
            public const string AREA = "#autoLOC_462420";
            public const string MASS = "#autoLOC_462423";
            public const string GRAV_PARAMETER = "#autoLOC_462426";
            public const string GRAVITY_ASL = "#autoLOC_462429";
            public const string ESCAPE_VELOCITY = "#autoLOC_462432";
            public const string ROTATION_PERIOD = "#autoLOC_462435";
            public const string SOI = "#autoLOC_462438";

            // Parameter names (atmospheric
            public const string ATMOSPHERE_CHARACTERISTICS_HEADER = "#autoLOC_462406";
            public const string ATMOSPHERE_PRESENT = "#autoLOC_462448";
            public const string ATMOSPHERE_HEIGHT = "#autoLOC_462453";
            public const string ATMOSPHERE_PRESSURE = "#autoLOC_462456";
            public const string ATMOSPHERE_ASL_TEMP = "#autoLOC_462459";

            // PlanetInfoPlus tags
            public const string PHYSICAL_SETTINGS_LABEL = "#PlanetInfoPlus_physicalSettingsLabel";
            public const string ATMOSPHERIC_SETTINGS_LABEL = "#PlanetInfoPlus_atmosphericSettingsLabel";
            public const string MAX_ELEVATION = "#PlanetInfoPlus_maxElevation";
            public const string SYNCHRONOUS_ALTITUDE = "#PlanetInfoPlus_synchronousAltitude";
            public const string ORBITAL_PERIOD = "#PlanetInfoPlus_orbitalPeriod";
        }
    }
}
