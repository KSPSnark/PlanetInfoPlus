namespace PlanetInfoPlus
{
    /// <summary>
    /// Various numeric string formats loaded from the config file.
    /// </summary>
    internal static class NumericFormats
    {
        // Name of the relevant config section for this class.
        public const string CONFIG_NODE_NAME = "NumericFormats";

        public static readonly NumberLocalizer BigNumber           = new NumberLocalizer("BigNumber");
        public static readonly NumberLocalizer EquatorialRadius    = new NumberLocalizer("EquatorialRadiusKm");
        public static readonly NumberLocalizer Gravity             = new NumberLocalizer("Gravity");
        public static readonly NumberLocalizer EscapeVelocity      = new NumberLocalizer("EscapeVelocity");
        public static readonly NumberLocalizer SOI                 = new NumberLocalizer("SoiKm");
        public static readonly NumberLocalizer MaxElevation        = new NumberLocalizer("MaxElevationM");
        public static readonly NumberLocalizer SynchronousAltitude = new NumberLocalizer("SynchronousAltitudeKm");
        public static readonly NumberLocalizer AtmosphereHeight    = new NumberLocalizer("AtmosphereHeightM");
        public static readonly NumberLocalizer AtmospherePressure  = new NumberLocalizer("AtmospherePressure");
        public static readonly NumberLocalizer Temperature         = new NumberLocalizer("Temperature");
        public static readonly NumberLocalizer NearSpaceHeight = new NumberLocalizer("NearSpaceHeightKm");

        public static readonly TimeLocalizer OrbitPeriod    = new TimeLocalizer("OrbitPeriodPrecision");
        public static readonly TimeLocalizer RotationPeriod = new TimeLocalizer("RotationPeriodPrecision");


        private static readonly NumberLocalizer[] localizers =
        {
            BigNumber,
            EquatorialRadius,
            Gravity,
            EscapeVelocity,
            SOI,
            MaxElevation,
            SynchronousAltitude,
            AtmosphereHeight,
            AtmospherePressure,
            Temperature,
            NearSpaceHeight,
        };

        private static readonly TimeLocalizer[] timeLocalizers =
        {
            OrbitPeriod,
            RotationPeriod
        };

        /// <summary>
        /// Load config values.
        /// </summary>
        /// <param name="node"></param>
        public static void LoadConfig(ConfigNode config)
        {
            for (int i = 0; i < config.values.Count; i++)
            {
                ConfigNode.Value entry = config.values[i];
                bool found = false;
                for (int j = 0; j < localizers.Length; j++)
                {
                    if (entry.name == localizers[j].configName)
                    {
                        found = true;
                        localizers[j].format = entry.value;
                        Logging.Log("Numeric format " + entry.name + " = " + entry.value);
                        continue;
                    }
                }
                if (!found)
                {
                    for (int j = 0; j < timeLocalizers.Length; j++)
                    {
                        if (entry.name == timeLocalizers[j].configName)
                        {
                            found = true;
                            timeLocalizers[j].precision = int.Parse(entry.value);
                            Logging.Log("Time formatter " + entry.name + " = " + entry.value);
                            continue;
                        }
                    }
                }
                if (!found)
                {
                    Logging.Warn("Found unknown numeric format config entry '" + config.name + "', ignoring");
                }
            }

            for (int i = 0; i < localizers.Length; i++)
            {
                if (string.IsNullOrEmpty(localizers[i].format))
                {
                    Logging.Warn("No numeric format config found for '" + localizers[i].configName + "'! Default formatting will apply.");
                }
            }
        }

        public class NumberLocalizer
        {
            
            public readonly string configName;
            public string format = null;

            internal NumberLocalizer(string configName)
            {
                this.configName = configName;
            }

            public string Localize(double value)
            {
                return (string.IsNullOrEmpty(format)) ? value.ToString() : KSPUtil.LocalizeNumber(value, format);
            }
        }

        public class TimeLocalizer
        {
            public readonly string configName;
            public int precision = 3;

            internal TimeLocalizer(string configName)
            {
                this.configName = configName;
            }

            public string Localize(double durationSeconds)
            {
                return KSPUtil.PrintTime(durationSeconds, precision, false);
            }
        }
    }
}
