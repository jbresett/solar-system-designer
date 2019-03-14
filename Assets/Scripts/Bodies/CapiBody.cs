using System.Collections;
using System.Collections.Generic;
using SimCapi;
using UnityEngine;

public class CapiBody : VisualBody {

    new public bool Active
    {
        get { return base.Active; }
        set {
            base.Active = active;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiActive.setValue(value);
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
        }
    }
    private SimCapiEnum<BodyType> capiType;
    
    new public string Name
    {
        get { return name; }
        set
        {
            base.Name = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiName.setValue(value);
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
            capiPosition.setValue(value);
        }
    }
    private SimCapiVector capiPosition;

    /// <summary>
    /// Initial position of the Body. Using resetPosition() will move the Body back to it's initial position.
    /// </summary>
    new public Vector3d InitialPosition
    {
        get { return initialPosition; }
        set
        {
            base.InitialPosition = value;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            capiInitialPosition.setValue(value);
        }
    }
    private SimCapiVector capiInitialPosition;

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
        }
    }
    private SimCapiNumber capiRotation;

    // Checks if CapiBody has been initated.

    new public void Awake()
    {
        base.Awake();
    }

    public void Init(int id)
    {
        capiActive = new SimCapiBoolean(active);
        capiActive.expose(id + " Active", false, false);
        capiActive.setChangeDelegate(
            delegate (bool value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Active = value;
                }
            }
        );

        capiName = new SimCapiString(name);
        capiName.expose(id + " Name", false, false);
        capiName.setChangeDelegate(
            delegate (string value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Name = value;
                }
            }
        );

        capiType = new SimCapiEnum<BodyType>(type);
        capiType.expose(id + " Type", false, false);
        capiType.setChangeDelegate(
            delegate (BodyType value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Type = value;
                }
            }
        );

        capiPosition = new SimCapiVector(id + " Position", base.position);
        capiInitialPosition = new SimCapiVector(id + " InitialPosition", base.initialPosition);

        capiMass = new SimCapiNumber((float)mass);
        capiMass.expose(id + " Mass", false, false);
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
        capiDiameter.expose(id + " Diameter", false, false);
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
        capiRotation.expose(id + " Rotation", false, false);
        capiRotation.setChangeDelegate(
            delegate (float value, SimCapi.ChangedBy changedBy)
            {
                if (changedBy == ChangedBy.AELP)
                {
                    Rotation = value;
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
