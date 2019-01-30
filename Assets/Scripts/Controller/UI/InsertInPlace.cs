using System;
using Planets;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class places an orbital body a specified postion
/// </summary>
public class InsertInPlace : MonoBehaviour
{
    public Button button;
    public TMP_InputField name;
    public TMP_Dropdown type;
    public TMP_InputField xPos;
    public TMP_InputField yPos;
    public TMP_InputField zPos;
    public TMP_InputField xVel;
    public TMP_InputField yVel;
    public TMP_InputField zVel;
    public TMP_InputField radius;
    public TMP_InputField mass;
    public GameObject planetBase;
    int x = 0;

    /// <summary>
    /// initializes class and begins listening for mouse click
    /// </summary>
    void Start()
    {
        button.onClick.AddListener(insert);
    }

    /// <summary>
    /// This function places each body in the array in its correct position
    /// </summary>
    public void insert()
    {
        GameObject obj = Bodies.activateNext();
        Body script = obj.GetComponent<Body>();
        script.Name = name.text;

        try
        {
            script.InitialPosition = new Vector3d(double.Parse(xPos.text), double.Parse(yPos.text), double.Parse(zPos.text));
        }
        catch (Exception)
        {
            script.InitialPosition = new Vector3d(0.0, 0.0, 0.0);
            Debugger.log("Invalid Position for Insert. Using base of (0,0,0)");
        }
        script.Position = script.InitialPosition;

        try
        { 
            script.Vel = new Vector3d(double.Parse(xVel.text), double.Parse(yVel.text), double.Parse(zVel.text));
        } catch (Exception) {
            script.Vel = new Vector3d(0.0,0.0,0.0);
            Debugger.log("Invalid Velocity for Insert. Using base of (0,0,0)");
        }
       
        script.Diameter = double.Parse(radius.text);
        script.Mass = convertMassUnits(double.Parse(mass.text));
        //script.Type = (BodyType)System.Enum.Parse(typeof(BodyType), type.options[type.value].text);
        script.Type = type.options[type.value].text.Enum<BodyType>();
    }

    /// <summary>
    /// scales radius
    /// </summary>
    private double convertRadiUnits(double value)
    {
        return value * 6.3781;
    }

    /// <summary>
    /// scales position
    /// </summary>
    private double[] convertPosUnits(double x, double y, double z)
    {
        double au = 14959.78707;
        double[] pos = {x * au, y * au, z * au};
        return pos;
    }

    /// <summary>
    /// scales mass
    /// </summary>
    private double convertMassUnits(double mass)
    {
        return mass * (5.9736 * Math.Pow(10, 24));
    }
}