namespace PlanetInfoPlus
{
    /// <summary>
    /// User settings for this mod, accessible through the settings menu for the game.
    ///
    /// Invaliable advice about how to make this work can be found here:
    /// https://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/
    /// </summary>
    internal class AtmosphericSettings : GameParameters.CustomParameterNode
    {
        // GameParameters.CustomParameterNode boilerplate
        public override string Title => Strings.ATMOSPHERE_CHARACTERISTICS_HEADER_LONG;
        public override string DisplaySection => PlanetInfoPlus.MOD_NAME;
        public override string Section => PlanetInfoPlus.MOD_NAME;
        public override int SectionOrder => 2;
        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;
        public override bool HasPresets => false;


        /// <summary>
        /// Get the global instance of this settings object.
        /// </summary>
        public static AtmosphericSettings Instance
        {
            get { return HighLogic.CurrentGame.Parameters.CustomParams<AtmosphericSettings>(); }
        }


        // Settings

        [GameParameters.CustomStringParameterUI(Strings.Tags.ATMOSPHERIC_SETTINGS_LABEL, lines = 3)]
        public string dummy = "";

        [GameParameters.CustomParameterUI(Strings.Tags.ATMOSPHERE_PRESENT)]
        public bool showPresent = true;

        [GameParameters.CustomParameterUI(Strings.Tags.ATMOSPHERE_HEIGHT)]
        public bool showHeight = true;

        [GameParameters.CustomParameterUI(Strings.Tags.ATMOSPHERE_PRESSURE)]
        public bool showPressure = true;

        [GameParameters.CustomParameterUI(Strings.Tags.ATMOSPHERE_ASL_TEMP)]
        public bool showTemperature = true;
    }
}
