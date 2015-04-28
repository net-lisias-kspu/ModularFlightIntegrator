﻿using System.Collections.Generic;
using UnityEngine;

class ModularFlightIntegrator : FlightIntegrator
{

    public delegate void voidDelegate(ModularFlightIntegrator fi);
    public delegate void voidPartDelegate(ModularFlightIntegrator fi, Part part);
    public delegate double doublePartDelegate(ModularFlightIntegrator fi, Part part);
    public delegate void voidThermalDataDelegate(ModularFlightIntegrator fi, PartThermalData ptd);
    public delegate double IntegratePhysicalObjectsDelegate(ModularFlightIntegrator fi, List<GameObject> pObjs, double atmDensity);


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

    public Vessel Vessel
    {
        get { return vessel; }
        set { vessel = value; }
    }

    public PhysicsGlobals.LiftingSurfaceCurve LiftCurves
    {
        get { return liftCurves; }
        set { liftCurves = value; }
    }

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

    public bool IsAnalytical
    {
        get { return isAnalytical; }
        set { isAnalytical = value; }
    }

    public double WarpReciprocal
    {
        get { return warpReciprocal; }
        set { warpReciprocal = value; }
    }

    public bool WasMachConvectionEnabled
    {
        get { return wasMachConvectionEnabled; }
        set { wasMachConvectionEnabled = value; }
    }


    // Awake fire when getting to the Flight Scene, not sooner
    protected void Awake()
    {
        VesselModuleManager.RemoveModuleOfType(typeof (FlightIntegrator));
        string msg = "Awake. Current modules coVesselModule : \n";
        foreach (var vesselModuleWrapper in VesselModuleManager.GetModules(false, false))
        {
            msg += "  " + vesselModuleWrapper.type.ToString() + " active=" + vesselModuleWrapper.active + " order=" + vesselModuleWrapper.order + "\n";
        }
        print(msg);

    }

    protected override void Start()
    {
        string msg = "Start. Current modules coVesselModule : \n";
        foreach (var vesselModuleWrapper in VesselModuleManager.GetModules(false, false))
        {
            msg += "  " + vesselModuleWrapper.type.ToString() + " active=" + vesselModuleWrapper.active + " order=" + vesselModuleWrapper.order + "\n";
        }
        print(msg);
        base.Start();
    }

    protected override void OnDestroy()
    {
        print("OnDestroy");
        base.OnDestroy();
    }

    protected override void HookVesselEvents()
    {
        print("HookVesselEvents");
        base.HookVesselEvents();
    }

    protected override void UnhookVesselEvents()
    {
        print("UnhookVesselEvents");
        base.UnhookVesselEvents();
    }

    protected override void FixedUpdate()
    {
        // print("FixedUpdate");

        // Update vessel values

        // UpdateThermodynamics

        // Copy values to part

        // UpdateOcclusion

        // Integrate Root part

        // IntegratePhysicalObjects

        base.FixedUpdate();
    }

    private static voidDelegate updateThermodynamicsOverride;
    private static voidDelegate updateThermodynamicsPre;
    private static voidDelegate updateThermodynamicsPost;

    public static bool RegisterUpdateThermodynamicsOverride(voidDelegate dlg)
    {
        if (HighLogic.LoadedScene != GameScenes.SPACECENTER)
        {
            print("You can only register on the SPACECENTER scene");
        }


        if (updateThermodynamicsOverride == null)
        {
            updateThermodynamicsOverride = dlg;
            return true;
        }

        print("UpdateThermodynamics already has an override");
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

    private static voidDelegate updateOcclusionOverride;

    protected override void UpdateOcclusion()
    {
        if (updateOcclusionOverride == null)
        {
            base.UpdateOcclusion();
        }
        else
        {
            updateOcclusionOverride(this);
        }
    }


    private static voidPartDelegate integrateOverride;
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


    private static IntegratePhysicalObjectsDelegate integratePhysicalObjectsOverride;
    protected override void IntegratePhysicalObjects(List<GameObject> pObjs, double atmDensity)
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

    private static voidPartDelegate updateAerodynamicsOverride;
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


    private static doublePartDelegate calculateDragValueOverride;
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

    private static voidDelegate updateThermalGraphOverride;
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

    private static voidDelegate updateConductionOverride;
    protected override void UpdateConduction()
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

    private static voidThermalDataDelegate updateConvectionOverride;
    protected override void UpdateConvection(PartThermalData ptd)
    {
        if (updateConvectionOverride == null)
        {
            base.UpdateConvection(ptd);
        }
        else
        {
            updateConvectionOverride(this, ptd);
        }
    }

    private static voidThermalDataDelegate updateRadiationOverride;
    protected override void UpdateRadiation(PartThermalData ptd)
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

    protected override double CalculateDragValue_Spherical(Part part)
    {
        return base.CalculateDragValue_Spherical(part);
    }

    protected override double CalculateDragValue_Cylindrical(Part part)
    {
        return base.CalculateDragValue_Cylindrical(part);
    }

    protected override double CalculateDragValue_Conic(Part part)
    {
        return base.CalculateDragValue_Conic(part);
    }

    protected override double CalculateDragValue_Cube(Part part)
    {
        return base.CalculateDragValue_Cube(part);
    }

    private static doublePartDelegate calculateAerodynamicAreaOverride;
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

    private static doublePartDelegate calculateAreaRadiativeOverride;
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

    private static doublePartDelegate calculateAreaExposedOverride;
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


    static void print(string msg)
    {
        MonoBehaviour.print("[ModularFlightIntegrator] " + msg);
    }


}
