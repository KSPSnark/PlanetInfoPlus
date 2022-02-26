using System.Collections.Generic;
using System.IO;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Console command for dumping data to a file.
    /// </summary>
    internal class ConsoleDumpCommand : DebugConsole.ConsoleCommand
    {
        private const string CONFIG_DUMP_FILE = PlanetInfoPlus.MOD_NAME + "Dump.cfg";
        private static readonly string filePath = Path.Combine(Path.GetDirectoryName(typeof(PlanetInfoScenario).Assembly.Location), CONFIG_DUMP_FILE);

        public ConsoleDumpCommand() : base("dump", "Dump planet max elevation data to file " + CONFIG_DUMP_FILE) { }

        public override void Call(string[] arguments)
        {
            Logging.Log("Checking elevation calculation for all celestial bodies...");
            CelestialBodyElevationScanner.Precalculate(-1);

            // The config dump file will be in the same folder as this assembly
            StreamWriter writer = File.CreateText(filePath);
            writer.WriteLine("// " + CONFIG_DUMP_FILE);
            writer.WriteLine("// This file is auto-generated and will be overwritten. Do not hand-edit.");
            writer.WriteLine("//");
            writer.WriteLine("// Highest points of KSP celestial bodies, as calculated by " + PlanetInfoPlus.MOD_NAME);
            writer.WriteLine("//");
            writer.WriteLine("// " + SurfacePoint.maxPlanetElevations.Count + " bodies present in file");
            List<string> bodies = new List<string>(SurfacePoint.maxPlanetElevations.Keys);
            bodies.Sort();
            foreach (string body in bodies)
            {
                SurfacePoint point = SurfacePoint.maxPlanetElevations[body];
                writer.WriteLine();
                writer.WriteLine("MAX_ELEVATION");
                writer.WriteLine("{");
                writer.WriteLine("    name = " + body);
                writer.WriteLine("    elevation = " + point.altitude);
                writer.WriteLine("    latitude = " + point.latitude);
                writer.WriteLine("    longitude = " + point.longitude);
                writer.WriteLine("}");
            }
            writer.Close();
            Logging.Log("Wrote " + SurfacePoint.maxPlanetElevations.Count + " bodies' data to " + CONFIG_DUMP_FILE);
        }
    }
}
