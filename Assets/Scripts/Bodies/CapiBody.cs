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
            capiType[Id].setValue(type);
        }
    }
    static private SimCapiEnum<BodyType>[] capiType;
    
    new public string Name
    {
        get { return name; }
        set
        {
            base.Name = value;
            capiName[Id].setValue(value);
        }
    }
    static private SimCapiString[] capiName;

    /// <summary>
    /// Mass of the body in Earths.
    /// </summary>
    new public double Mass
    {
        get { return mass; }
        set
        {
            base.Mass = value;
            capiMass[Id].setValue((float)value);
        }
    }
    static private SimCapiNumber[] capiMass;

    /// <summary>
    /// Diameter in Earths.
    /// </summary>
    new public double Diameter
    {
        get { return diameter; }
        set
        {
            base.Diameter = value;
            capiDiameter[Id].setValue((float)value);
        }
    }
    static private SimCapiNumber[] capiDiameter;

    /// <summary>
    /// Current Position of the object. Any changes will be reflected in Unity.
    /// </summary>
    new public Vector3d Position
    {
        get { return position; }
        set
        {
            base.Position = value;
            capiPosition[Id].getList().Clear();
            capiPosition[Id].getList().Add(value.x.ToString());
            capiPosition[Id].getList().Add(value.y.ToString());
            capiPosition[Id].getList().Add(value.z.ToString());
            capiPosition[Id].updateValue();
        }
    }
    static private SimCapiStringArray[] capiPosition;

    /// <summary>
    /// Initial position of the Body. Using resetPosition() will move the Body back to it's initial position.
    /// </summary>
    new public Vector3d InitialPosition
    {
        get { return initialPosition; }
        set
        {
            base.InitialPosition = value;
            capiInitialPosition[Id].getList().Clear();
            capiInitialPosition[Id].getList().Add(value.x.ToString());
            capiInitialPosition[Id].getList().Add(value.y.ToString());
            capiInitialPosition[Id].getList().Add(value.z.ToString());
            capiInitialPosition[Id].updateValue();
        }
    }
    static private SimCapiStringArray[] capiInitialPosition;

    /// <summary>
    /// Planet's rotational speed, in earth days.
    /// </summary>
    new public double Rotation
    {
        get { return rotation; }
        set
        {
            base.Rotation = value;
            capiRotation[Id].setValue((float)value);
        }
    }
    static private SimCapiNumber[] capiRotation;

    // Checks if CapiBody has been initated.
    static private bool hasInit = false;

    /// <summary>
    /// Initiates CapiBody, exposing all variables to the main Capi system.
    /// </summary>
    static public void Init()
    {
        // Prevent double initiation.
        if (hasInit) return;
        hasInit = true;

        capiName = new SimCapiString[Bodies.MAX];
        capiType = new SimCapiEnum<BodyType>[Bodies.MAX];
        capiPosition = new SimCapiStringArray[Bodies.MAX];
        capiInitialPosition = new SimCapiStringArray[Bodies.MAX];
        capiMass = new SimCapiNumber[Bodies.MAX];
        capiDiameter = new SimCapiNumber[Bodies.MAX];
        capiRotation = new SimCapiNumber[Bodies.MAX];

        for (int i = 0; i < Bodies.MAX; i++)
        {

            // Create Capi values and expose.

            capiName[i] = new SimCapiString("");
            capiName[i].expose(i + " Name", false, false);
            
            capiType[i] = new SimCapiEnum<BodyType>(BodyType.Undefined);
            capiType[i].expose(i + " Type", false, false);
            
            capiPosition[i] = new SimCapiStringArray();
            capiPosition[i].expose(i + " Position", false, false);

            capiInitialPosition[i] = new SimCapiStringArray();
            capiInitialPosition[i].expose(i + " InitialPosition", false, false);

            capiMass[i] = new SimCapiNumber(0);
            capiMass[i].expose(i + " Mass", false, false);

            capiDiameter[i] = new SimCapiNumber(0);
            capiDiameter[i].expose(i + " Diameter", false, false);

            capiRotation[i] = new SimCapiNumber(0);
            capiRotation[i].expose(i + " Rotation", false, false);

            // Set Deligates
            /* [TODO] Deligates don't work as is: to fix on later task.
            capiName[i].setChangeDelegate(
                delegate (string value, SimCapi.ChangedBy changedBy)
                {
                    Bodies.get(i).Name = value;
                }
            );
            capiType[i].setChangeDelegate(
                delegate (BodyType value, SimCapi.ChangedBy changedBy)
                {
                    Bodies.get(i).Type = value;
                }
            );

            capiPosition[i].setChangeDelegate(
                delegate (string[] values, SimCapi.ChangedBy changedBy)
                {
                    Bodies.get(i).Position = new Vector3d(
                            System.Convert.ToDouble(values[0]),
                            System.Convert.ToDouble(values[1]),
                            System.Convert.ToDouble(values[2])
                    );
                }
            );

            capiInitialPosition[i].setChangeDelegate(
                delegate (string[] values, SimCapi.ChangedBy changedBy)
                {
                    Bodies.get(i).InitialPosition = new Vector3d(
                            System.Convert.ToDouble(values[0]),
                            System.Convert.ToDouble(values[1]),
                            System.Convert.ToDouble(values[2])
                    );
                }
            );

            capiMass[i].setChangeDelegate(
                delegate (float value, SimCapi.ChangedBy changeBy)
                {
                    Bodies.get(i).Mass = value;
                }
            );

            capiDiameter[i].setChangeDelegate(
                delegate (float value, SimCapi.ChangedBy changeBy)
                {
                    Bodies.get(i).Diameter = value;
                }
            );

            capiRotation[i].setChangeDelegate(
                delegate (float value, SimCapi.ChangedBy changeBy)
                {
                    Bodies.get(i).Rotation = value;
                }
            );
            */
        }

        Debugger.log("Body elements exposed.");
    }

    new public void Awake()
    {
        base.Awake();
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
