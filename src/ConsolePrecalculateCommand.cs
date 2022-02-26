namespace PlanetInfoPlus
{
    /// <summary>
    /// Force pre-calculation of all bodies' max elevation.
    /// </summary>
    internal class ConsolePrecalculateCommand : DebugConsole.ConsoleCommand
    {
        public ConsolePrecalculateCommand() : base("precalc", "Force immediate pre-calculation of all bodies' max elevation") { }

        public override void Call(string[] arguments)
        {
            CelestialBodyElevationScanner.Precalculate(-1);
        }
    }
}
