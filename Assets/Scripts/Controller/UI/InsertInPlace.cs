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
    public GameObject bodyBase;
    public TMP_InputField objName;
    public TMP_Dropdown type;
    public TMP_InputField radius;
    public TMP_InputField mass;
    public TMP_InputField xPos;
    public TMP_InputField yPos;
    public TMP_InputField zPos;
    public TMP_InputField xVel;
    public TMP_InputField yVel;
    public TMP_InputField zVel;

    public GameObject UseParticleSystem;

    /// <summary>
    /// initializes class and begins listening for mouse click
    /// </summary>
    void Start()
    {
        button.onClick.AddListener(insert);
        type.onValueChanged.AddListener(delegate { updateUnits(); });
    }

    private void updateUnits()
    {
    TextMeshProUGUI[] textComps = this.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
        Debug.Log(textComps.Length);
        String unit = type.options[type.value].text;
        foreach (var text in textComps)
        {
            if (text.tag.ToLower().Equals("dist"))
            {
                text.text = unit;
            }

            if (text.tag.ToLower().Equals("mass"))
            {
                text.text = unit;
            }
        }
    }

    /// <summary>
    /// This function places each body in the array in its correct position
    /// </summary>
    public void insert()
    {
        GameObject obj = Sim.Bodies.activateNext();
        Body script = obj.GetComponent<Body>();
        script.Name = objName.text;

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
            script.Velocity = new Vector3d(double.Parse(xVel.text), double.Parse(yVel.text), double.Parse(zVel.text));
        } catch (Exception) {
            script.Velocity = new Vector3d(0.0,0.0,0.0);
            Debugger.log("Invalid Velocity for Insert. Using base of (0,0,0)");
        }
       
        script.Diameter = double.Parse(radius.text);
        script.Mass = double.Parse(mass.text);
        //script.Type = (BodyType)System.Enum.Parse(typeof(BodyType), type.options[type.value].text);
        script.Type = type.options[type.value].text.Enum<BodyType>();

        //obj.AddComponent<InsertParticleSystem>();
        //Instantiate(InsertParticleSystem);
        //InsertParticleSystem.
        //Instantiate(UseParticleSystem, obj.transform);
        //particleSystem.transform.parent = obj.transform;
        //particleSystem.SetActive(true);
    }

    /// <summary>
    /// scales radius
    /// </summary>
    private double convertRadiUnits(double value)
    {
        return value * 6.3781;
    }

}