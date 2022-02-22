namespace PlanetInfoPlus
{
    /// <summary>
    /// User settings for this mod, accessible through the settings menu for the game.
    /// 
    /// Invaliable advice about how to make this work can be found here:
    /// https://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/
    /// </summary>
    public class PhysicalSettings : GameParameters.CustomParameterNode
    {
        // GameParameters.CustomParameterNode boilerplate
        public override string Title => Strings.PHYSICAL_CHARACTERISTICS_HEADER;
        public override string DisplaySection => PlanetInfoPlus.MOD_NAME;
        public override string Section => PlanetInfoPlus.MOD_NAME;
        public override int SectionOrder => 1;
        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;
        public override bool HasPresets => false;


        /// <summary>
        /// Get the global instance of this settings object.
        /// </summary>
        public static PhysicalSettings Instance
        {
            get { return HighLogic.CurrentGame.Parameters.CustomParams<PhysicalSettings>(); }
        }


        // Settings

        [GameParameters.CustomStringParameterUI(Strings.Tags.PHYSICAL_SETTINGS_LABEL, lines = 3)]
        public string dummy = "";

        [GameParameters.CustomParameterUI(Strings.Tags.EQ_RADIUS)]
        public bool showEquatorialRadius = true;

        [GameParameters.CustomParameterUI(Strings.Tags.AREA)]
        public bool showArea = false;

        [GameParameters.CustomParameterUI(Strings.Tags.MASS)]
        public bool showMass = false;

        [GameParameters.CustomParameterUI(Strings.Tags.GRAV_PARAMETER)]
        public bool showGravParameter = false;

        [GameParameters.CustomParameterUI(Strings.Tags.GRAVITY_ASL)]
        public bool showGravityASL = true;

        [GameParameters.CustomParameterUI(Strings.Tags.ESCAPE_VELOCITY)]
        public bool showEscapeVelocity = true;

        [GameParameters.CustomParameterUI(Strings.Tags.ROTATION_PERIOD)]
        public bool showRotationPeriod = true;

        [GameParameters.CustomParameterUI(Strings.Tags.SOI)]
        public bool showSOI = true;

        [GameParameters.CustomParameterUI(Strings.Tags.SYNCHRONOUS_ALTITUDE)]
        public bool showSynchronousAltitude = true;

        [GameParameters.CustomParameterUI(Strings.Tags.ORBITAL_PERIOD)]
        public bool showOrbitalPeriod = true;
    }
}
