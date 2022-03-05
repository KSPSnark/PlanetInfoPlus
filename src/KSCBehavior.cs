using UnityEngine;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Runs once in KSC.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    internal class KSCBehavior : MonoBehaviour
    {
        /// <summary>
        /// Here once when the KSC scene starts up for the first time in a play session.
        /// </summary>
        public void Start()
        {
            if (PhysicalSettings.Instance.showMaxElevation)
            {
                // Spend a few moments pre-calculating highest elevations on the most
                // relevant celestial bodies. Rationale: This is happening at scene-switch
                // time, when the player is probably waiting several seconds anwyway,
                // so they probably won't even notice if it takes just a little longer
                // to start up. On the other hand, anything that we *don't* pre-calculate
                // will end up being an interruption of a second or so when they go to the
                // planetarium view for the first time for a particular body, which is
                // probably more of a noticeable interruption of gameplay.
                //
                // Note that this precalculation is cached across play sessions, so
                // it won't do this on *every* startup; it'll only do this until
                // everything is calculated. Should be only a few scene switches.
                CelestialBodyElevationScanner.Precalculate(4000);
            }
        }
    }
}
