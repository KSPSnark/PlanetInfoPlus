namespace PlanetInfoPlus
{
    /// <summary>
    /// Color values loaded from the config file.
    /// </summary>
    internal static class InfoColors
    {
        // Name of the relevant config section for this class.
        public const string CONFIG_NODE_NAME = "Colors";

        public static readonly Colorizer Default = new Colorizer("Default");
        public static readonly Colorizer Highlight = new Colorizer("Highlight");
        public static readonly Colorizer Attention = new Colorizer("Attention");

        private static readonly Colorizer[] colorizers =
        {
            Default,
            Highlight,
            Attention
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
                for (int j = 0; j < colorizers.Length; j++)
                {
                    if (entry.name == colorizers[j].configName)
                    {
                        found = true;
                        colorizers[j].color = entry.value;
                        Logging.Log("Color '" + entry.name + "' = " + entry.value);
                        continue;
                    }
                }
                if (!found)
                {
                    Logging.Warn("Found unknown color config entry '" + config.name + "', ignoring");
                }
            }

            for (int i = 0; i < colorizers.Length; i++)
            {
                if (string.IsNullOrEmpty(colorizers[i].color))
                {
                    Logging.Warn("No color config found for '" + colorizers[i].configName + "'! Default color will apply.");
                }
            }
        }

        public static string Colorize(string text, string color)
        {
            return (string.IsNullOrEmpty(color)) ? text : ("<color=" + color + ">" + text + "</color>");
        }

        public class Colorizer
        {

            public readonly string configName;
            public string color = null;

            internal Colorizer(string configName)
            {
                this.configName = configName;
            }

            public string Colorize(string text)
            {
                return InfoColors.Colorize(text, color);
            }
        }
    }
}
