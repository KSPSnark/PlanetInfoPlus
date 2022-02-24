/// <summary>
/// User settings for this mod, accessible through the settings menu for the game.
///
/// Invaliable advice about how to make this work can be found here:
/// https://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/
/// </summary>
namespace PlanetInfoPlus
{
    internal class GameplaySettings : GameParameters.CustomParameterNode
    {
        // GameParameters.CustomParameterNode boilerplate
        public override string Title => Strings.GAMEPLAY_CHARACTERISTICS_HEADER;
        public override string DisplaySection => PlanetInfoPlus.MOD_NAME;
        public override string Section => PlanetInfoPlus.MOD_NAME;
        public override int SectionOrder => 3;
        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;
        public override bool HasPresets => false;

        /// <summary>
        /// Get the global instance of this settings object.
        /// </summary>
        public static GameplaySettings Instance
        {
            get { return HighLogic.CurrentGame.Parameters.CustomParams<GameplaySettings>(); }
        }

        // Settings
        public bool IsAnyActive => showUpperAtmosphereHeight
            || showNearSpaceHight
            || showBiomeCount
            || showExploration;

        [GameParameters.CustomStringParameterUI(Strings.Tags.GAMEPLAY_SETTINGS_LABEL, lines = 3)]
        public string dummy = "";


        [GameParameters.CustomParameterUI(Strings.Tags.UPPER_ATMOSPHERE_HEIGHT)]
        public bool showUpperAtmosphereHeight = true;

        [GameParameters.CustomParameterUI(Strings.Tags.NEAR_SPACE_HEIGHT)]
        public bool showNearSpaceHight = true;

        [GameParameters.CustomParameterUI(Strings.Tags.BIOME_COUNT)]
        public bool showBiomeCount = true;

        [GameParameters.CustomParameterUI(Strings.Tags.EXPLORATION)]
        public bool showExploration = true;
    }
}
