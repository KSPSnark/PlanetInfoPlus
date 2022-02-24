using UnityEngine;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Runs once when the game hits the main menu on startup. Does one-time startup
    /// stuff, such as loading configs.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class Loader : MonoBehaviour
    {
        private const string MASTER_NODE_NAME = PlanetInfoPlus.MOD_NAME;

        private delegate void ConfigLoader(ConfigNode node);

        /// <summary>
        /// Here when the script starts up.
        /// </summary>
        public void Start()
        {
            UrlDir.UrlConfig[] configs = GameDatabase.Instance.GetConfigs(MASTER_NODE_NAME);
            if ((configs == null) || (configs.Length == 0)) {
                Logging.Error("No " + MASTER_NODE_NAME + " config found. Default values will be used.");
                return;
            }
            ProcessMasterNode(configs[0].config);
        }

        /// <summary>
        /// Process the main PlanetInfoPlus config node.
        /// </summary>
        /// <param name="masterNode"></param>
        private static void ProcessMasterNode(ConfigNode masterNode)
        {
            TryProcessChildNode(masterNode, NumericFormats.CONFIG_NODE_NAME, NumericFormats.LoadConfig);
            TryProcessChildNode(masterNode, InfoColors.CONFIG_NODE_NAME, InfoColors.LoadConfig);
            TryProcessChildNode(masterNode, ConfigSettings.CONFIG_NODE_NAME, ConfigSettings.LoadConfig);
        }

        /// <summary>
        /// Looks for a child with the specified name, and delegates to it if found.
        /// </summary>
        /// <param name="masterNode"></param>
        /// <param name="childName"></param>
        /// <param name="loader"></param>
        private static void TryProcessChildNode(ConfigNode masterNode, string childName, ConfigLoader loader)
        {
            ConfigNode child = masterNode.nodes.GetNode(childName);
            if (child == null)
            {
                Logging.Warn("Child node " + childName + " of master config node " + MASTER_NODE_NAME + " not found, skipping");
            }
            else
            {
                Logging.Log("Loading " + masterNode.name + " config: " + child.name);
                loader(child);
            }
        }
    }
}
