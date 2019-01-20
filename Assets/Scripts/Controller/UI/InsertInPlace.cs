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
        String bodyName;
        double result = 0.0;
        planetBase = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Models/Planet_in_AU_Units.obj", typeof(GameObject));
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("OrbitalBody");
        Debug.Log(bodies);
        if(name.text == ""){
            bodyName = name.text;
        } else {
            bodyName = "UnnamedBody";
        }
        int id = 1;
        foreach (var b in bodies)
        {
            if (b.name == bodyName)
            {
                bodyName = name.text + id;
                id++;
            }
        }
        //Vector3 pos = new Vector3();
        //Quaternion rot = new Quaternion(0,0,0,0);
        GameObject body = Instantiate(planetBase);
        body.SetActive(true);
        body.name = bodyName;
        OrbitalBody script = body.AddComponent<OrbitalBody>();
        if(double.TryParse(xVel.text,out result) == true && double.TryParse(yVel.text,out result) && double.TryParse(zVel.text,out result)){
            script.Vel = new Vector3d(double.Parse(xVel.text), double.Parse(yVel.text), double.Parse(zVel.text));
        } else {
            script.Vel = new Vector3d(0.0,0.0,0.0);
        }
        /*
        if(double.TryParse(xPos.text,out result) == true && double.TryParse(yPos.text,out result) && double.TryParse(zPos.text,out result)){
            script.setPos(convertPosUnits(double.Parse(xPos.text),double.Parse(yPos.text),double.Parse(zPos.text)));
        } else {
            foreach(var body in bodies){
                if(pos)
                script.setPos = new Vector3d(0.0,0.0,0.0);
            }
        }*/
        script.Radius = (float)convertRadiUnits(double.Parse(radius.text));
        script.Mass = (float)convertMassUnits(double.Parse(mass.text));
        script.Type = type.options[type.value].text;
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