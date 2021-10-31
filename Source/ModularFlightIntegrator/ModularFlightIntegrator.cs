/*
	This file is part of Modular Flight Integrator /L Unleashed
		© 2019-2021 Lisias T : http://lisias.net <support@lisias.net>
		© 2014-2018 Sarbian

	Modular Flight Integrator /L is double licensed, as follows:

		* SKL 1.0 : https://ksp.lisias.net/SKL-1_0.txt
		* GPL 2.0 : https://www.gnu.org/licenses/gpl-2.0.txt

	And you are allowed to choose the License that better suit your needs.

	Modular Flight Integrator /L Unleashed is distributed in the hope that
	it will be useful, but WITHOUT ANY WARRANTY; without even the implied
	warranty of	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

	You should have received a copy of the SKL Standard License 1.0
	along with Modular Flight Integrator /L Unleashed.
	If not, see <https://ksp.lisias.net/SKL-1_0.txt>.

	You should have received a copy of the GNU General Public License 2.0
	along with Modular Flight Integrator /L Unleashed.
	If not, see <https://www.gnu.org/licenses/>.

---- To satisfy the previous MIT Licensing terms ----

Copyright (c) 2014 sarbian

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

*/
using System.Collections.Generic;
using UnityEngine;

namespace ModularFI
{
    // 9 is the Timing of the stock FlightIntegrator but using it leads to race/exec order problems with FAR+Principia.
    [DefaultExecutionOrder(10)]
    public class ModularFlightIntegrator : FlightIntegrator
    {

        public delegate void voidDelegate(ModularFlightIntegrator fi);
        public delegate void voidBoolDelegate(ModularFlightIntegrator fi, bool b);
        public delegate double doubleDelegate(ModularFlightIntegrator fi);
        public delegate double doubleDoubleDelegate(ModularFlightIntegrator fi, double d);
        public delegate void voidPartDelegate(ModularFlightIntegrator fi, Part part);
        public delegate double doublePartDelegate(ModularFlightIntegrator fi, Part part);
        public delegate void voidThermalDataDelegate(ModularFlightIntegrator fi, PartThermalData ptd);
        public delegate double doubleThermalDataDelegate(ModularFlightIntegrator fi, PartThermalData ptd);
        public delegate double IntegratePhysicalObjectsDelegate(ModularFlightIntegrator fi, List<physicalObject> pObjs, double atmDensity);
        
        // Properties to access the FlightIntegrator protected field
        // Some should be readonly I guess

        public Transform IntegratorTransform
        {
            get { return integratorTransform; }
            set { integratorTransform = value; }
        }

        public Part PartRef
        {
            get { return partRef; }
            set { partRef = value; }
        }

        public CelestialBody CurrentMainBody
        {
            get { return currentMainBody; }
            set { currentMainBody = value; }
        }

        /*public Vessel Vessel
        {
            get { return vessel; }
            set { vessel = value; }
        }*/
        
        public double DensityThermalLerp
        {
            get { return densityThermalLerp; }
            set { densityThermalLerp = value; }
        }

        public int PartCount
        {
            get { return partCount; }
            set { partCount = value; }
        }

        public static int SunLayerMask
        {
            get { return sunLayerMask; }
            set { sunLayerMask = value; }
        }

        public List<PartThermalData> PartThermalDataList
        {
            get { return partThermalDataList; }
        }

        public bool RecreateThermalGraph
        {
            get { return recreateThermalGraph; }
            set { recreateThermalGraph = value; }
        }

        public int PartThermalDataCount
        {
            get { return partThermalDataCount; }
            set { partThermalDataCount = value; }
        }
        
        public double FDeltaTime
        {
            get { return fDeltaTime; }
            // set { fDeltaTime = value; } // I feel a set here is a recipe for disaster
        }

        public double FDeltaTimeRecip
        {
            get { return fDeltaTimeRecip; }
            set { fDeltaTimeRecip = value; }
        }

        public double DeltaTime
        {
            get { return deltaTime; }
            set { deltaTime = value; }
        }

        public double FTimeSinceThermo
        {
            get { return fTimeSinceThermo; }
            set { fTimeSinceThermo = value; }
        }

        public double FTimeSinceThermoRecip
        {
            get { return fTimeSinceThermoRecip; }
            set { fTimeSinceThermoRecip = value; }
        }
        
        public bool WasMachConvectionEnabled
        {
            get { return wasMachConvectionEnabled; }
            set { wasMachConvectionEnabled = value; }
        }

        protected override void OnStart()
        {
            Log.trace("MFI Start");
            base.OnStart();

            string msg = "Start. VesselModule on vessel : \n";
            foreach (VesselModule vm in vessel.gameObject.GetComponents<VesselModule>())
            {
                msg += "  " + vm.GetType().Name + "\n";
            }
            // Register our replacement FixedUpdate to run at the same timing as the stock FlightIntegrator
            //TimingManager.UpdateAdd(TimingManager.TimingStage.FlightIntegrator, TimedUpdate);
            //TimingManager.FixedUpdateAdd(TimingManager.TimingStage.FlightIntegrator, TimedFixedUpdate);
            Log.detail(msg);
        }

        //
        //protected override void HookVesselEvents()
        //{
        //    Log.trace("HookVesselEvents");
        //    base.HookVesselEvents();
        //}
        //
        //protected override void UnhookVesselEvents()
        //{
        //    Log.trace("UnhookVesselEvents");
        //    base.UnhookVesselEvents();
        //}
        //

        // TODO : VesselPrecalculate
        //protected override void VesselPrecalculate()
        //{
        //}
        private static voidDelegate fixedUpdateOverride;

        public static bool RegisterFixedUpdateOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (fixedUpdateOverride == null)
            {
                fixedUpdateOverride = dlg;
                return true;
            }

            Log.warn("FixedUpdate already has an override");
            return false;
        }

        // We want the FixedUpdate code to run exactly when we want (after vessel and partmodule).
        // The stock game uses a Unity settings to force the execution order
        // Mods can not use that but can register with TimingManager.FixedUpdateAdd, called in OnStart here.
        // replace our FixedUpdate with something that does nothing since we will call TimedFixedUpdate
        protected override void FixedUpdate()
        {
            if (fixedUpdateOverride == null)
            {
                base.FixedUpdate();
            }
            else
            {
                fixedUpdateOverride(this);
            }
        }
        
        private static doubleDelegate calculateShockTemperatureOverride;

        public static bool RegisterCalculateShockTemperature(doubleDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateShockTemperatureOverride == null)
            {
                calculateShockTemperatureOverride = dlg;
                return true;
            }

            Log.warn("CalculateShockTemperature already has an override");
            return false;
        }

        public override double CalculateShockTemperature()
        {
            if (calculateShockTemperatureOverride == null)
            {
                return base.CalculateShockTemperature();
            }
            else
            {
                return calculateShockTemperatureOverride(this);
            }
        }

        public double BaseFICalculateShockTemperature()
        {
            return base.CalculateShockTemperature();
        }
        
        private static voidDelegate updateThermodynamicsOverride;
        private static voidDelegate updateThermodynamicsPre;
        private static voidDelegate updateThermodynamicsPost;

        public static bool RegisterUpdateThermodynamicsOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateThermodynamicsOverride == null)
            {
                updateThermodynamicsOverride = dlg;
                return true;
            }

            Log.warn("UpdateThermodynamics already has an override");
            return false;
        }

        public static void RegisterUpdateThermodynamicsPre(voidDelegate dlg)
        {
            updateThermodynamicsPre += dlg;
        }

        public static void RegisterUpdateThermodynamicsPost(voidDelegate dlg)
        {
            updateThermodynamicsPost += dlg;
        }

        protected override void UpdateThermodynamics()
        {
            // UpdateThermalGraph

            // UpdateConduction

            // UpdateConvection

            // UpdateRadiation

            if (updateThermodynamicsPre != null)
            {
                updateThermodynamicsPre(this);
            }

            if (updateThermodynamicsOverride == null)
            {
                base.UpdateThermodynamics();
            }
            else
            {
                updateThermodynamicsOverride(this);
            }

            if (updateThermodynamicsPost != null)
            {
                updateThermodynamicsPost(this);
            }

        }

        public void BaseFIUpdateThermodynamics()
        {
            base.UpdateThermodynamics();
        }

        //TODO : public virtual void ThermalIntegrationPass(bool averageWithPrevious)

        private static doubleDelegate calculateAnalyticTemperatureOverride;

        public static bool RegisterCalculateAnalyticTemperature(doubleDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateAnalyticTemperatureOverride == null)
            {
                calculateAnalyticTemperatureOverride = dlg;
                return true;
            }

            Log.warn("CalculateAnalyticTemperature already has an override");
            return false;
        }

        public override double CalculateAnalyticTemperature()
        {
            if (calculateAnalyticTemperatureOverride == null)
            {
                return base.CalculateAnalyticTemperature();
            }
            else
            {
                return calculateAnalyticTemperatureOverride(this);
            }
        }

        public double BaseFICalculateAnalyticTemperature()
        {
            return base.CalculateAnalyticTemperature();
        }

        private static voidBoolDelegate updateOcclusionOverride;

        public static bool RegisterUpdateOcclusionOverride(voidBoolDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateOcclusionOverride == null)
            {
                updateOcclusionOverride = dlg;
                return true;
            }

            Log.warn("UpdateOcclusion already has an override");
            return false;
        }

        protected override void UpdateOcclusion(bool all)
        {
            if (updateOcclusionOverride == null)
            {
                base.UpdateOcclusion(all);
            }
            else
            {
                updateOcclusionOverride(this, all);
            }
        }

        public void BaseFIUpdateOcclusion(bool all)
        {
            base.UpdateOcclusion(all);
        }

        private static voidPartDelegate integrateOverride;

        public static bool RegisterIntegrateOverride(voidPartDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (integrateOverride == null)
            {
                integrateOverride = dlg;
                return true;
            }
            
            Log.warn("Integrate already has an override");
            return false;
        }

        protected override void Integrate(Part part)
        {
            // Aply Gravity / centrifugal / Coriolis Forces)

            // UpdateAerodynamics

            // Integrate child parts

            if (integrateOverride == null)
            {
                base.Integrate(part);
            }
            else
            {
                integrateOverride(this, part);
            }
        }

        public void BaseFIIntegrate(Part part)
        {
            base.Integrate(part);
        }

        private static IntegratePhysicalObjectsDelegate integratePhysicalObjectsOverride;

        public static bool RegisterIntegratePhysicalObjectsOverride(IntegratePhysicalObjectsDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (integratePhysicalObjectsOverride == null)
            {
                integratePhysicalObjectsOverride = dlg;
                return true;
            }

            Log.warn("IntegratePhysicalObjects already has an override");
            return false;
        }

        protected override void IntegratePhysicalObjects(List<physicalObject> pObjs, double atmDensity)
        {
            if (integratePhysicalObjectsOverride == null)
            {
                base.IntegratePhysicalObjects(pObjs, atmDensity);
            }
            else
            {
                integratePhysicalObjectsOverride(this, pObjs, atmDensity);
            }
        }

        public void BaseFIIntegratePhysicalObjects(List<physicalObject> pObjs, double atmDensity)
        {
            base.IntegratePhysicalObjects(pObjs, atmDensity);
        }

        private static voidDelegate calculatePressureOverride;

        public static bool RegisterCalculatePressureOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculatePressureOverride == null)
            {
                calculatePressureOverride = dlg;
                return true;
            }

            Log.warn("CalculatePressure already has an override");
            return false;
        }

        protected override void CalculatePressure()
        {
            if (calculatePressureOverride == null)
            {
                base.CalculatePressure();
            }
            else
            {
                calculatePressureOverride(this);
            }
        }

        public void BaseFICalculatePressure()
        {
            base.CalculatePressure();
        }
        
        private static voidDelegate calculateSunBodyFluxOverride;
        private static voidDelegate calculateSunBodyFluxPre;
        private static voidDelegate calculateSunBodyFluxPost;

        public static bool RegisterCalculateSunBodyFluxOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateSunBodyFluxOverride == null)
            {
                calculateSunBodyFluxOverride = dlg;
                return true;
            }

            Log.warn("CalculateSunBodyFlux already has an override");
            return false;
        }

        public static void RegisterCalculateSunBodyFluxPre(voidDelegate dlg)
        {
            calculateSunBodyFluxPre += dlg;
        }

        public static void RegisterCalculateSunBodyFluxPost(voidDelegate dlg)
        {
            calculateSunBodyFluxPost += dlg;
        }

        protected override void CalculateSunBodyFlux()
        {
            if (calculateSunBodyFluxPre != null)
            {
                calculateSunBodyFluxPre(this);
            }

            if (calculateSunBodyFluxOverride == null)
            {
                base.CalculateSunBodyFlux();
            }
            else
            {
                calculateSunBodyFluxOverride(this);
            }

            if (calculateSunBodyFluxPost != null)
            {
                calculateSunBodyFluxPost(this);
            }
        }

        public void BaseFICalculateSunBodyFlux()
        {
            base.CalculateSunBodyFlux();
        }


        // TODO : CalculateDensityThermalLerp
        private static doubleDelegate calculateDensityThermalLerpOverride;

        public static bool RegisterCalculateDensityThermalLerpOverride(doubleDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateDensityThermalLerpOverride == null)
            {
                calculateDensityThermalLerpOverride = dlg;
                return true;
            }

            Log.warn("CalculateDensityThermalLerp already has an override");
            return false; 
        }

        public override double CalculateDensityThermalLerp()
        {
            if (calculateDensityThermalLerpOverride == null)
            {
                return base.CalculateDensityThermalLerp();
            }
            else
            {
                return calculateDensityThermalLerpOverride(this);
            }
        }

        // TODO : CalculateBackgroundRadiationTemperature
        private static doubleDoubleDelegate calculateBackgroundRadiationTemperatureOverride;

        public static bool RegisterCalculateBackgroundRadiationTemperatureOverride(doubleDoubleDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateBackgroundRadiationTemperatureOverride == null)
            {
                calculateBackgroundRadiationTemperatureOverride = dlg;
                return true;
            }

            Log.warn("CalculateBackgroundRadiationTemperature already has an override");
            return false;
        }

        protected override double CalculateBackgroundRadiationTemperature(double ambientTemp)
        {
            if (calculateBackgroundRadiationTemperatureOverride == null)
            {
                return base.CalculateBackgroundRadiationTemperature(ambientTemp);
            }
            else
            {
                return calculateBackgroundRadiationTemperatureOverride(this, ambientTemp);
            }
        }
        // TODO : CalculateConstantsVacuum
        private static voidDelegate calculateConstantsVacuumOverride;

        public static bool RegisterCalculateConstantsVacuumOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateConstantsVacuumOverride == null)
            {
                calculateConstantsVacuumOverride = dlg;
                return true;
            }

            Log.warn("CalculateConstantsVacuum already has an override");
            return false;
        }

        protected override void CalculateConstantsVacuum()
        {
            if (calculateConstantsVacuumOverride == null)
            {
                base.CalculateConstantsVacuum();
            }
            else
            {
                calculateConstantsVacuumOverride(this);
            }
        }
        // TODO : CalculateConstantsAtmosphere
        private static voidDelegate calculateConstantsAtmosphereOverride;

        public static bool RegistercalculateConstantsAtmosphereOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateConstantsAtmosphereOverride == null)
            {
                calculateConstantsAtmosphereOverride = dlg;
                return true;
            }

            Log.warn("CalculateConstantsAtmosphere already has an override");
            return false;
        }

        protected override void CalculateConstantsAtmosphere()
        {
            if (calculateConstantsAtmosphereOverride == null)
            {
                base.CalculateConstantsAtmosphere();
            }
            else
            {
                calculateConstantsAtmosphereOverride(this);
            }
        }

        // TODO : CalculateConvectiveCoefficient
        private static doubleDelegate calculateConvectiveCoefficientOverride;

        public static bool RegisterCalculateConvectiveCoefficientOverride(doubleDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateConvectiveCoefficientOverride == null)
            {
                calculateConvectiveCoefficientOverride = dlg;
                return true;
            }

            Log.warn("CalculateConvectiveCoefficient already has an override");
            return false;
        }

        protected override double CalculateConvectiveCoefficient()
        {
            if (calculateConvectiveCoefficientOverride == null)
            {
                return base.CalculateConvectiveCoefficient();
            }
            else
            {
                return calculateConvectiveCoefficientOverride(this);
            }
        }
        
        // TODO : CalculateConvectiveCoefficientNewtonian
        private static doubleDelegate calculateConvectiveCoefficientNewtonianOverride;

        public static bool RegisterCalculateConvectiveCoefficientNewtonianOverride(doubleDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateConvectiveCoefficientNewtonianOverride == null)
            {
                calculateConvectiveCoefficientNewtonianOverride = dlg;
                return true;
            }

            Log.warn("CalculateConvectiveCoefficientNewtonian already has an override");
            return false;
        }

        protected override double CalculateConvectiveCoefficientNewtonian()
        {
            if (calculateConvectiveCoefficientNewtonianOverride == null)
            {
                return base.CalculateConvectiveCoefficientNewtonian();
            }
            else
            {
                return calculateConvectiveCoefficientNewtonianOverride(this);
            }
        }
        // TODO : CalculateConvectiveCoefficientMach
        private static doubleDelegate calculateConvectiveCoefficientMachOverride;

        public static bool RegisterCalculateConvectiveCoefficientMachOverride(doubleDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateConvectiveCoefficientMachOverride == null)
            {
                calculateConvectiveCoefficientMachOverride = dlg;
                return true;
            }

            Log.warn("CalculateConvectiveCoefficientMach already has an override");
            return false;
        }

        protected override double CalculateConvectiveCoefficientMach()
        {
            if (calculateConvectiveCoefficientMachOverride == null)
            {
                return base.CalculateConvectiveCoefficientMach();
            }
            else
            {
                return calculateConvectiveCoefficientMachOverride(this);
            }
        }

        private static voidPartDelegate updateAerodynamicsOverride;

        public static bool RegisterUpdateAerodynamicsOverride(voidPartDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateAerodynamicsOverride == null)
            {
                updateAerodynamicsOverride = dlg;
                return true;
            }

            Log.warn("UpdateAerodynamics already has an override");
            return false;
        }

        protected override void UpdateAerodynamics(Part part)
        {
            // CalculateDragValue

            // CalculateAerodynamicArea
            // CalculateAreaRadiative
            // CalculateAreaExposed
            if (updateAerodynamicsOverride == null)
            {
                base.UpdateAerodynamics(part);
            }
            else
            {
                updateAerodynamicsOverride(this, part);
            }
        }

        public void BaseFIUpdateAerodynamics(Part part)
        {
            base.UpdateAerodynamics(part);
        }

        private static doublePartDelegate calculateDragValueOverride;

        public static bool RegisterCalculateDragValueOverride(doublePartDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateDragValueOverride == null)
            {
                calculateDragValueOverride = dlg;
                return true;
            }

            Log.warn("CalculateDragValue already has an override");
            return false;
        }

        protected override double CalculateDragValue(Part part)
        {
            // CalculateDragValue_Spherical
            // CalculateDragValue_Cylindrical
            // CalculateDragValue_Conic
            // CalculateDragValue_Cube

            if (calculateDragValueOverride == null)
            {
                return base.CalculateDragValue(part);
            }
            else
            {
                return calculateDragValueOverride(this, part);
            }
        }

        public double BaseFICalculateDragValue(Part part)
        {
            return base.CalculateDragValue(part);
        }

        private static voidDelegate updateThermalGraphOverride;

        public static bool RegisterUpdateThermalGraphOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateThermalGraphOverride == null)
            {
                updateThermalGraphOverride = dlg;
                return true;
            }

            Log.warn("UpdateThermalGraph already has an override");
            return false;
        }

        protected override void UpdateThermalGraph()
        {
            if (updateThermalGraphOverride == null)
            {
                base.UpdateThermalGraph();
            }
            else
            {
                updateThermalGraphOverride(this);
            }
        }

        public void BaseFIUpdateThermalGraph()
        {
            base.UpdateThermalGraph();
        }

        
        private static voidDelegate updateCompoundPartsOverride;

        public static bool RegisterUpdateCompoundParts(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateCompoundPartsOverride == null)
            {
                updateCompoundPartsOverride = dlg;
                return true;
            }

            Log.warn("UpdateCompoundParts already has an override");
            return false;
        }

        public override void UpdateCompoundParts()
        {
            if (updateCompoundPartsOverride == null)
            {
                base.UpdateCompoundParts();
            }
            else
            {
                updateCompoundPartsOverride(this);
            }
        }

        public void BaseFIUpdateCompoundParts()
        {
            base.UpdateCompoundParts();
        }
        
        private static voidThermalDataDelegate setSkinPropertiesOverride;

        public static bool RegisterSetSkinProperties(voidThermalDataDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (setSkinPropertiesOverride == null)
            {
                setSkinPropertiesOverride = dlg;
                return true;
            }

            Log.warn("UpdateCompoundParts already has an override");
            return false;
        }

        public override void SetSkinProperties(PartThermalData ptd)
        {
            if (setSkinPropertiesOverride == null)
            {
                base.SetSkinProperties(ptd);
            }
            else
            {
                setSkinPropertiesOverride(this, ptd);
            }
        }

        public void BaseFIetSkinPropertie(PartThermalData ptd)
        {
            base.SetSkinProperties(ptd);
        }

        private static voidDelegate updateConductionOverride;

        public static bool RegisterUpdateConductionOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateConductionOverride == null)
            {
                updateConductionOverride = dlg;
                return true;
            }

            Log.warn("UpdateConduction already has an override");
            return false;
        }

        public override void UpdateConduction()
        {
            if (updateConductionOverride == null)
            {
                base.UpdateConduction();
            }
            else
            {
                updateConductionOverride(this);
            }
        }

        public void BaseFIUpdateConduction()
        {
            base.UpdateConduction();
        }


        // TODO : public virtual double GetUnifiedSkinTemp

        // TODO : UnifySkinTemp

        private static voidThermalDataDelegate updateConvectionOverride;

        public static bool RegisterUpdateConvectionOverride(voidThermalDataDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateConvectionOverride == null)
            {
                updateConvectionOverride = dlg;
                return true;
            }

            Log.warn("UpdateConvection already has an override");
            return false;
        }

        //protected override void UpdateConvection(PartThermalData ptd)
        //{
        //    if (updateConvectionOverride == null)
        //    {
        //        base.UpdateConvection(ptd);
        //    }
        //    else
        //    {
        //        updateConvectionOverride(this, ptd);
        //    }
        //}
        //
        //public void BaseFIUpdateConvection(PartThermalData ptd)
        //{
        //    base.UpdateConvection(ptd);
        //}

        private static voidThermalDataDelegate updateRadiationOverride;

        public static bool RegisterUpdateRadiationOverride(voidThermalDataDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateRadiationOverride == null)
            {
                updateRadiationOverride = dlg;
                return true;
            }

            Log.warn("UpdateConvection already has an override");
            return false;
        }

        public override void UpdateRadiation(PartThermalData ptd)
        {
            if (updateRadiationOverride == null)
            {
                base.UpdateRadiation(ptd);
            }
            else
            {
                updateRadiationOverride(this, ptd);
            }
        }

        public void BaseFIUpdateRadiation(PartThermalData ptd)
        {
            base.UpdateRadiation(ptd);
        }

        private static voidDelegate updateMassStatsOverride;

        public static bool RegisterUpdateMassStatsOverride(voidDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateMassStatsOverride == null)
            {
                updateMassStatsOverride = dlg;
                return true;
            }
            Log.warn("UpdateMassStats already has an override");
            return false;
        }

        protected override void UpdateMassStats()
        {
            if (updateMassStatsOverride == null)
            {
                base.UpdateMassStats();
            }
            else
            {
                updateMassStatsOverride(this);
            }
        }

        public void BaseFIUpdateMassStats()
        {
            base.UpdateMassStats();
        }

        private static doubleThermalDataDelegate updateGetSunAreaOverride;

        public static bool RegisterGetSunAreaOverride(doubleThermalDataDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (updateGetSunAreaOverride == null)
            {
                updateGetSunAreaOverride = dlg;
                return true;
            }

            Log.warn("GetSunArea already has an override");
            return false;
        }

        public override double GetSunArea(PartThermalData ptd)
        {
            if (updateGetSunAreaOverride == null)
            {
                return base.GetSunArea(ptd);
            }
            else
            {
                return updateGetSunAreaOverride(this, ptd);
            }
        }

        public double BaseFIGetSunArea(PartThermalData ptd)
        {
            return base.GetSunArea(ptd);
        }

        //

        private static doubleThermalDataDelegate getBodyAreaOverride;

        public static bool RegisterGetBodyAreaOverride(doubleThermalDataDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (getBodyAreaOverride == null)
            {
                getBodyAreaOverride = dlg;
                return true;
            }

            Log.warn("GetBodyArea already has an override");
            return false;
        }

        public override double GetBodyArea(PartThermalData ptd)
        {
            if (getBodyAreaOverride == null)
            {
                return base.GetBodyArea(ptd);
            }
            else
            {
                return getBodyAreaOverride(this, ptd);
            }
        }

        public double BaseFIBodyArea(PartThermalData ptd)
        {
            return base.GetBodyArea(ptd);
        }

        //protected override double CalculateDragValue_Spherical(Part part)
        //{
        //    return base.CalculateDragValue_Spherical(part);
        //}
        //
        //protected override double CalculateDragValue_Cylindrical(Part part)
        //{
        //    return base.CalculateDragValue_Cylindrical(part);
        //}
        //
        //protected override double CalculateDragValue_Conic(Part part)
        //{
        //    return base.CalculateDragValue_Conic(part);
        //}
        //
        //protected override double CalculateDragValue_Cube(Part part)
        //{
        //    return base.CalculateDragValue_Cube(part);
        //}

        private static doublePartDelegate calculateAerodynamicAreaOverride;

        public static bool RegisterCalculateAerodynamicAreaOverride(doublePartDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateAerodynamicAreaOverride == null)
            {
                calculateAerodynamicAreaOverride = dlg;
                return true;
            }

            Log.warn("CalculateAerodynamicArea already has an override");
            return false;
        }

        protected override double CalculateAerodynamicArea(Part part)
        {
            if (calculateAerodynamicAreaOverride == null)
            {
                return base.CalculateAerodynamicArea(part);
            }
            else
            {
                return calculateAerodynamicAreaOverride(this, part);
            }
        }

        public double BaseFICalculateAerodynamicArea(Part part)
        {
            return base.CalculateAerodynamicArea(part);
        }

        private static doublePartDelegate calculateAreaRadiativeOverride;

        public static bool RegisterCalculateAreaRadiativeOverride(doublePartDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateAreaRadiativeOverride == null)
            {
                calculateAreaRadiativeOverride = dlg;
                return true;
            }

            Log.warn("CalculateAreaRadiative already has an override");
            return false;
        }

        protected override double CalculateAreaRadiative(Part part)
        {
            if (calculateAreaRadiativeOverride == null)
            {
                return base.CalculateAreaRadiative(part);
            }
            else
            {
                return calculateAreaRadiativeOverride(this, part);
            }
        }

        public double BaseFICalculateAreaRadiative(Part part)
        {
            return base.CalculateAreaRadiative(part);
        }

        private static doublePartDelegate calculateAreaExposedOverride;

        public static bool RegisterCalculateAreaExposedOverride(doublePartDelegate dlg)
        {
            if (!CheckSpaceCenter()) return false;

            if (calculateAreaExposedOverride == null)
            {
                calculateAreaExposedOverride = dlg;
                return true;
            }

            Log.warn("CalculateAreaExposed already has an override");
            return false;
        }

        protected override double CalculateAreaExposed(Part part)
        {
            if (calculateAreaExposedOverride == null)
            {
                return base.CalculateAreaExposed(part);
            }
            else
            {
                return calculateAreaExposedOverride(this, part);
            }
        }

        public double BaseFICalculateAreaExposed(Part part)
        {
            return base.CalculateAreaExposed(part);
        }

        public double BaseFIGetPhysicslessChildMass(Part part)
        {
            return base.GetPhysicslessChildMass(part);
        }

        private static bool CheckSpaceCenter()
        {
            if (GameScenes.SPACECENTER == HighLogic.LoadedScene) return true;

            Log.error(typeof(ModularFlightIntegrator), "You can only register on the SPACECENTER scene");
            return false;
        }
    }
}
