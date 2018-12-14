using System;
using Planets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InsertInPlace : MonoBehaviour
{
    public Button button;
    new public InputField name;
    public Dropdown type;
    public InputField xPos;
    public InputField yPos;
    public InputField zPos;
    public InputField xVel;
    public InputField yVel;
    public InputField zVel;
    public InputField radius;
    public InputField mass;
    public GameObject planetBase;

    void Start()
    {
        button.onClick.AddListener(insert);
    }
    public void insert()
    {
        GameObject[] bodies = GameObject.FindGameObjectsWithTag("OrbitalBody");
        Debug.Log(bodies);
        String bodyName = name.text;
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
        script.Vel = new Vector3d(double.Parse(xVel.text), double.Parse(yVel.text), double.Parse(zVel.text));
        //script.setPos(convertPosUnits(double.Parse(xPos.text),double.Parse(yPos.text),double.Parse(zPos.text)));
        script.Radius = (float)convertRadiUnits(double.Parse(radius.text));
        script.Mass = (float)convertMassUnits(double.Parse(mass.text));
        script.Type = type.options[type.value].text;
    }

    private double convertRadiUnits(double value)
    {
        return value * 6.3781;
    }

    private double[] convertPosUnits(double x, double y, double z)
    {
        double au = 14959.78707;
        double[] pos = {x * au, y * au, z * au};
        return pos;
    }

    private double convertMassUnits(double mass)
    {
        return mass * (5.9736 * Math.Pow(10, 24));
    }
}