using System.Collections;
using System.Collections.Generic;
using SimCapi;
using UnityEngine;

public class CapiBody : VisualBody {

    new public BodyType Type
    {
        get { return type; }
        set
        {
            base.Type = value;
            capiType.setValue(type);
        }
    }
    private SimCapiEnum<BodyType> capiType;
    
    new public string Name
    {
        get { return name; }
        set
        {
            base.Name = value;
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
            capiMass.setValue((float)value);
        }
    }
    private SimCapiNumber capiMass;

    /// <summary>
    /// Diameter in Earths.
    /// </summary>
    new public double Diameter
    {
        get { return diameter; }
        set
        {
            base.Diameter = value;
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
            capiPosition.getList().Clear();
            capiPosition.getList().Add(value.x.ToString());
            capiPosition.getList().Add(value.y.ToString());
            capiPosition.getList().Add(value.z.ToString());
            capiPosition.updateValue();
        }
    }
    private SimCapiStringArray capiPosition;

    /// <summary>
    /// Initial position of the Body. Using resetPosition() will move the Body back to it's initial position.
    /// </summary>
    new public Vector3d InitialPosition
    {
        get { return initialPosition; }
        set
        {
            base.InitialPosition = value;
            capiInitialPosition.getList().Clear();
            capiInitialPosition.getList().Add(value.x.ToString());
            capiInitialPosition.getList().Add(value.y.ToString());
            capiInitialPosition.getList().Add(value.z.ToString());
            capiInitialPosition.updateValue();
        }
    }
    private SimCapiStringArray capiInitialPosition;

    /// <summary>
    /// Planet's rotational speed, in earth days.
    /// </summary>
    new public double Rotation
    {
        get { return rotation; }
        set
        {
            base.Rotation = value;
            capiRotation.setValue((float)value);
        }
    }
    private SimCapiNumber capiRotation;

    new public void Awake()
    {
        base.Awake();
        // Get Id
        //id = NBody.register(this);
        int id = 0;

        // Create Capi values and expose.
        capiName = new SimCapiString(name);
        capiName.expose(id + " Name", false, false);

        capiType = new SimCapiEnum<BodyType>(type);
        capiType.expose(id + " Type", false, false);

        capiPosition = new SimCapiStringArray();
        capiPosition.expose(id + " Position", false, false);

        capiInitialPosition = new SimCapiStringArray();
        capiInitialPosition.expose(id + " InitialPosition", false, false);

        capiMass = new SimCapiNumber((float)mass);
        capiMass.expose(id + " Mass", false, false);

        capiDiameter = new SimCapiNumber((float)diameter);
        capiDiameter.expose(id + " Diameter", false, false);

        capiRotation = new SimCapiNumber((float)rotation);
        capiRotation.expose(id + " Rotation", false, false);

        // Set Deligates

        capiName.setChangeDelegate(
            delegate (string value, SimCapi.ChangedBy changedBy)
            {
                Name = value;
            }
        );
        capiType.setChangeDelegate(
            delegate (BodyType value, SimCapi.ChangedBy changedBy)
            {
                Type = value;
            }
        );

        capiPosition.setChangeDelegate(
            delegate (string[] values, SimCapi.ChangedBy changedBy)
            {
                Position = new Vector3d(
                        System.Convert.ToDouble(values[0]),
                        System.Convert.ToDouble(values[1]),
                        System.Convert.ToDouble(values[2])
                );
            }
        );

        capiInitialPosition.setChangeDelegate(
            delegate (string[] values, SimCapi.ChangedBy changedBy)
            {
                InitialPosition = new Vector3d(
                        System.Convert.ToDouble(values[0]),
                        System.Convert.ToDouble(values[1]),
                        System.Convert.ToDouble(values[2])
                );
            }
        );

        capiMass.setChangeDelegate(
            delegate (float value, SimCapi.ChangedBy changeBy)
            {
                Mass = value;
            }
        );

        capiDiameter.setChangeDelegate(
            delegate (float value, SimCapi.ChangedBy changeBy)
            {
                Diameter = value;
            }
        );

        capiRotation.setChangeDelegate(
            delegate (float value, SimCapi.ChangedBy changeBy)
            {
                Rotation = value;
            }
        );

    }
    

    // Use this for initialization
    new public void Start () {
        base.Start();
    }
	
	// Update is called once per frame
	new public void Update () {
        base.Update();
    }
}
