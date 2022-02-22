using KSP.Localization;
using KSP.UI;
using KSP.UI.Screens;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetInfoPlus
{
    /// <summary>
    /// Rejiggers the "planet info" pane that pops up in planetarium view in KSP (tracking station, map view in flight).
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class PlanetInfoPlus : MonoBehaviour
    {
        // The name of the mod, used for display and logging purposes.
        internal const string MOD_NAME = "PlanetInfoPlus";

        public void Awake()
        {
            Logging.Log("Initializing");
            KbApp_PlanetParameters.CallbackAfterActivate += OnAppActivated;
        }

        /// <summary>
        /// This is called whenever the info pane "app" is activated.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool OnAppActivated(KbApp_PlanetParameters app, MapObject target)
        {
            Logging.Log("App activated for " + app.currentBody.GetDisplayName());

            // We'll clear out the list and rebuild it from scratch.
            app.appFrame.scrollList.Clear(true);
            app.cascadingList = Instantiate(app.cascadingListPrefab);
            app.cascadingList.Setup(app.appFrame.scrollList);
            app.cascadingList.transform.SetParent(app.transform, false);

            // Add the physical characteristics section.
            UnityEngine.UI.Button button;
            UIListItem physicalHeader = app.cascadingList.CreateHeader(Localizer.Format(Strings.PHYSICAL_CHARACTERISTICS_HEADER), out button, true);
            app.cascadingList.ruiList.AddCascadingItem(physicalHeader, app.cascadingList.CreateFooter(), CreatePhysicalCharacteristics(app), button);

            // Add the atmospheric characteristics section.
            UIListItem atmosphericHeader = app.cascadingList.CreateHeader(Localizer.Format(Strings.ATMOSPHERE_CHARACTERISTICS_HEADER), out button, true);
            app.cascadingList.ruiList.AddCascadingItem(atmosphericHeader, app.cascadingList.CreateFooter(), CreateAtmosphericCharacteristics(app), button);

            // Return value appears to be ignored.
            return true;
        }

        /// <summary>
        /// Create one item in the list of fields displayed, using a default color.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static UIListItem CreateBody(GenericCascadingList list, string key, string value)
        {
            return CreateBody(list, InfoColors.Default, key, value);
        }

        /// <summary>
        /// Create one item in the list of fields displayed, using the specified color.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="colorizer"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static UIListItem CreateBody(GenericCascadingList list, InfoColors.Colorizer colorizer, string key, string value)
        {
            return list.CreateBody(key, colorizer.Colorize(value));
        }

        //============================== PARAMETER SECTIONS ===========================================

        /// <summary>
        /// Get the list of fields with which to populate the "physical characteristics" section.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private static List<UIListItem> CreatePhysicalCharacteristics(KbApp_PlanetParameters app)
        {
            List<UIListItem> list = new List<UIListItem>();
            if (PhysicalSettings.Instance.showEquatorialRadius) list.Add(CreateParam_EqRadius(app));
            if (PhysicalSettings.Instance.showArea)             list.Add(CreateParam_Area(app));
            if (PhysicalSettings.Instance.showMass)             list.Add(CreateParam_Mass(app));
            if (PhysicalSettings.Instance.showGravParameter)    list.Add(CreateParam_GravParameter(app));
            if (PhysicalSettings.Instance.showGravityASL)       list.Add(CreateParam_GravityASL(app));
            if (PhysicalSettings.Instance.showEscapeVelocity)   list.Add(CreateParam_EscapeVelocity(app));
            if (PhysicalSettings.Instance.showRotationPeriod)   list.Add(CreateParam_RotationPeriod(app));
            if (PhysicalSettings.Instance.showSOI)              list.Add(CreateParam_SOI(app));
            if (PhysicalSettings.Instance.showMaxElevation)     list.Add(CreateParam_MaxElevation(app));
            if (PhysicalSettings.Instance.showSynchronousAltitude && app.currentBody.hasSolidSurface)
                list.Add(CreateParam_SynchronousAltitude(app));
            if (PhysicalSettings.Instance.showOrbitalPeriod && app.currentBody.HasOrbit())
                list.Add(CreateParam_OrbitalPeriod(app));

            return list;
        }

        /// <summary>
        /// Get the list of fields with which to populate the "atmospheric characteristics" section.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private static List<UIListItem> CreateAtmosphericCharacteristics(KbApp_PlanetParameters app)
        {
            List<UIListItem> list = new List<UIListItem>();
            if (AtmosphericSettings.Instance.showPresent) list.Add(CreateParam_HasAtmosphere(app));
            if (!app.currentBody.atmosphere) return list;

            if (AtmosphericSettings.Instance.showHeight)      list.Add(CreateParam_AtmosphereHeight(app));
            if (AtmosphericSettings.Instance.showPressure)    list.Add(CreateParam_AtmospherePressure(app));
            if (AtmosphericSettings.Instance.showTemperature) list.Add(CreateParam_AtmosphereTemperature(app));

            return list;
        }

        //====================== PHYSICAL CHARACTERISTICS PARAMETERS ==================================

        private static UIListItem CreateParam_EqRadius(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.EQ_RADIUS,
                NumericFormats.EquatorialRadius.Localize(0.001 * app.currentBody.Radius) + " " + Strings.KM);
        }

        private static UIListItem CreateParam_Area(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.AREA,
                NumericFormats.BigNumber.Localize(4.0 * Math.PI * app.currentBody.Radius * app.currentBody.Radius)
                    + " " + Strings.M2);
        }

        private static UIListItem CreateParam_Mass(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.MASS,
                NumericFormats.BigNumber.Localize(app.currentBody.Mass)
                    + " " + Strings.KG);
        }

        private static UIListItem CreateParam_GravParameter(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.GRAV_PARAMETER,
                NumericFormats.BigNumber.Localize(app.currentBody.gravParameter)
                    + " " + Strings.M3_PER_S2);
        }

        private static UIListItem CreateParam_GravityASL(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.GRAVITY_ASL,
                NumericFormats.Gravity.Localize(app.currentBody.GeeASL)
                    + " " + Strings.GRAVITIES);
        }

        private static UIListItem CreateParam_EscapeVelocity(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.ESCAPE_VELOCITY,
                NumericFormats.EscapeVelocity.Localize(Math.Sqrt(2.0 * app.currentBody.gravParameter / app.currentBody.Radius))
                    + " " + Strings.M_PER_S);
        }

        private static UIListItem CreateParam_RotationPeriod(KbApp_PlanetParameters app)
        {
            // For tidally locked bodies, if we're also going to be displaying the orbital
            // period, there's no point in displaying the rotation period, too.  Just show
            // it as "Locked".
            if (PhysicalSettings.Instance.showOrbitalPeriod && app.currentBody.tidallyLocked) return CreateBody(
                app.cascadingList,
                Strings.ROTATION_PERIOD,
                Strings.LOCKED);

            // Special handling for bodies with retrograde rotation.
            if (app.currentBody.rotationPeriod < 0)
            {
                return CreateBody(
                    app.cascadingList,
                    InfoColors.Attention,
                    Strings.RETROGRADE_ROTATION,
                    KSPUtil.PrintTime(-app.currentBody.rotationPeriod, 3, false));
            }

            // Either it's not tidally locked, or we're not showing the orbital period.
            // If the former, just display normally.  If the latter, show it with the
            // description "locked rotation" to make it clear that it's a tidally locked body.
            return CreateBody(
                app.cascadingList,
                app.currentBody.tidallyLocked ? Strings.LOCKED_ROTATION : Strings.ROTATION_PERIOD,
                KSPUtil.PrintTime(app.currentBody.rotationPeriod, 3, false));
        }

        private static UIListItem CreateParam_SOI(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.SOI,
                NumericFormats.SOI.Localize(0.001 * app.currentBody.sphereOfInfluence)
                    + " " + Strings.KM);
        }

        private static UIListItem CreateParam_MaxElevation(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.MAX_ELEVATION,
                NumericFormats.MaxElevation.Localize(app.currentBody.MaxElevation())
                    + " " + Strings.M);
        }

        private static UIListItem CreateParam_SynchronousAltitude(KbApp_PlanetParameters app)
        {
            double altitude = app.currentBody.GetSynchronousAltitude();
            string altitudeString = double.IsNaN(altitude)
                ? Strings.NOT_APPLICABLE
                : NumericFormats.SynchronousAltitude.Localize(altitude * 0.001) + " " + Strings.KM;
            return CreateBody(
                app.cascadingList,
                Strings.SYNCHRONOUS_ALTITUDE,
                altitudeString);
        }

        private static UIListItem CreateParam_OrbitalPeriod(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.ORBITAL_PERIOD,
                KSPUtil.PrintTime(app.currentBody.orbit.period, 3, false));
        }


        //===================== ATMOSPHERIC CHARACTERISTICS PARAMETERS ===============================

        private static UIListItem CreateParam_HasAtmosphere(KbApp_PlanetParameters app)
        {
            if (!app.currentBody.atmosphere) return CreateBody(app.cascadingList, Strings.ATMOSPHERE_PRESENT, Strings.NO);

            if (app.currentBody.atmosphereContainsOxygen)
            {
                return CreateBody(
                    app.cascadingList,
                    InfoColors.Highlight,
                    Strings.ATMOSPHERE_PRESENT,
                    Strings.YES + " " + Strings.OXYGENATED);
            }
            else
            {
                return CreateBody(app.cascadingList, Strings.ATMOSPHERE_PRESENT, Strings.YES);
            }
        }

        private static UIListItem CreateParam_AtmosphereHeight(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.ATMOSPHERE_HEIGHT,
                NumericFormats.AtmosphereHeight.Localize(app.currentBody.atmosphereDepth) + " " + Strings.M);
        }

        private static UIListItem CreateParam_AtmospherePressure(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.ATMOSPHERE_PRESSURE,
                NumericFormats.AtmospherePressure.Localize(app.currentBody.atmospherePressureSeaLevel * PhysicsGlobals.KpaToAtmospheres)
                    + " " + Strings.ATMOSPHERES);
        }

        private static UIListItem CreateParam_AtmosphereTemperature(KbApp_PlanetParameters app)
        {
            return CreateBody(
                app.cascadingList,
                Strings.ATMOSPHERE_ASL_TEMP,
                NumericFormats.Temperature.Localize(app.currentBody.atmosphereTemperatureSeaLevel)
                    + " " + Strings.K);
        }
    }
}
