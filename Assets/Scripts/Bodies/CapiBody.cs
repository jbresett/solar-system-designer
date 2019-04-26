using System.Collections;
using System.Collections.Generic;
using SimCapi;
using UnityEngine;

public class CapiBody : VisualBody {

    new public bool Active
    {
        get { return base.Active; }
        set {
            base.Active = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiActive.setValue(value);

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi(); 
        }
    }
    private SimCapiBoolean capiActive;

    new public BodyType Type
    {
        get { return type; }
        set
        {
            base.Type = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiType.setValue(value);

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();
        }
    }
    private SimCapiEnum<BodyType> capiType;
    
    new public BodyMaterial Material
    {
        get { return material; }
        set
        {
            base.Material = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiMaterial.setValue(value);

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();
        }
    }
    private SimCapiEnum<BodyMaterial> capiMaterial;


    new public string Name
    {
        get { return name; }
        set
        {
            base.Name = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiName.setValue(value);

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();
        }
    }
    private SimCapiString capiName;

    /// <summary>
    /// Mass of the body in Earths.
    /// </summary>
    new public double Mass
    {
        get { return mass; }
        set
        {
            base.Mass = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiMass.setValue((float)value);

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();
        }
    }
    private SimCapiNumber capiMass;

    /// <summary>// No Capi interface during edit mode.
    /// Diameter in Earths.
    /// </summary>
    new public double Diameter
    {
        get { return diameter; }
        set
        {
            base.Diameter = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiDiameter.setValue((float)value);

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();
        }
    }
    private SimCapiNumber capiDiameter;

    /// <summary>
    /// Current Position of the object. Any changes will be reflected in Unity.
    /// </summary>
    new public Vector3d Position
    {
        get { return position; }
        set
        {
            base.Position = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();

            // Set to update Capi to current value in 1 second.
            if (!positionDelayUpdate)
            {
                positionDelayUpdate = true;
                Invoke("CapiPositionUpdate", 1.0f);
            }
        }
    }
    private SimCapiVector capiPosition;

    // Handles Delayed update for position (maximum updates/second to display).
    private bool positionDelayUpdate = false;
    private void CapiPositionUpdate()
    {
        positionDelayUpdate = false;
        capiPosition.setValue(base.position);
    }

    /// <summary>
    /// Current Position of the object. Any changes will be reflected in Unity.
    /// </summary>
    private SimCapiVector capiVelocity;
    new public Vector3d Velocity
    {
        get { return velocity; }
        set
        {
            base.Velocity = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();

            // Set to update Capi to current value in 1 second   
            if (!velocityDelayUpdate)
            {
                velocityDelayUpdate = true;
                Invoke("CapiVelocityUpdate", 1.0f);
            }
        }
    }

    // Handles Delayed update for velocity (maximum updates/second to display).
    private bool velocityDelayUpdate = false;
    private void CapiVelocityUpdate()
    {
        velocityDelayUpdate = false;
        capiVelocity.setValue(base.velocity);
    }

    /// <summary>
    /// Planet's rotational speed, in earth days.
    /// </summary>
    new public double Rotation
    {
        get { return rotation; }
        set
        {
            base.Rotation = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiRotation.setValue((float)value);

            // Update Capi State immediatly if Simulation is paused.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();
        }
    }
    private SimCapiNumber capiRotation;


    private SimCapiBoolean capiAutoVelocity;
    new public bool InitialVelocity
    {
        get { return initialVelocity; }
        set
        {
            base.InitialVelocity = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            if (Sim.Settings.Paused) State.Instance.UpdateCapi();

            capiAutoVelocity.setValue(!value); // Auto = opposite of set value.
        }
    }

    new public void Awake()
    {
        base.Awake();
    }

    public void Init(int id)
    {
        Id = id;

        string baseName = "Body." + id;

        capiName = new SimCapiString(name);
        capiName.expose(baseName + ".Name", false, false);
        capiName.setChangeDelegate(
            delegate (string value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Name = value;
                }
            }
        );

        capiActive = new SimCapiBoolean(active);
        capiActive.expose(baseName + ".Active", false, false);
        capiActive.setChangeDelegate(
            delegate (bool value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Active = value;
                }
            }
        );

        capiType = new SimCapiEnum<BodyType>(type);
        capiType.expose(baseName + ".Type", false, false);
        capiType.setChangeDelegate(
            delegate (BodyType value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Type = value;
                }
            }
        );

        capiMaterial = new SimCapiEnum<BodyMaterial>(material);
        capiMaterial.expose(baseName + ".Material", false, false);
        capiMaterial.setChangeDelegate(
            delegate (BodyMaterial value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Material = value;
                }
            }
        );

        capiMass = new SimCapiNumber((float)mass);
        capiMass.expose(baseName + ".Mass", false, false);
        capiMass.setChangeDelegate(
            delegate (float value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Mass = value;
                }
            }
        );

        capiDiameter = new SimCapiNumber((float)diameter);
        capiDiameter.expose(baseName + ".Diameter", false, false);
        capiDiameter.setChangeDelegate(
            delegate (float value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Diameter = value;
                }
            }
        );

        capiRotation = new SimCapiNumber((float)rotation);
        capiRotation.expose(baseName + ".Rotation", false, false);
        capiRotation.setChangeDelegate(
            delegate (float value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Rotation = value;
                }
            }
        );

        // Set Capi Vectors. Exposure and Delegation are handled interally.
        capiPosition = new SimCapiVector(baseName + ".Position", Position);
        capiVelocity = new SimCapiVector(baseName + ".Velocity", Velocity);

        //Auto velocity
        capiAutoVelocity = new SimCapiBoolean(!initialVelocity);
        capiAutoVelocity.expose(baseName + ".Velocity.Auto", false, false);
        capiAutoVelocity.setChangeDelegate(
            delegate (bool value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    InitialVelocity = !value;
                }
            }
        );
    }

    // Use this for initialization
    new public void Start () {

    }
	
	// Update is called once per frame
	new public void Update () {
        base.Update();
    }
}
