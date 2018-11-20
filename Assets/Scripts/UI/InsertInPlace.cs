using Planets;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InsertInPlace : MonoBehaviour
{
    public Button button;
    public InputField name;
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
        
        Vector3 pos = new Vector3(float.Parse(xPos.text),float.Parse(yPos.text),float.Parse(zPos.text));
        Quaternion rot = new Quaternion(0,0,0,0);
        GameObject body = Instantiate(planetBase, pos, rot);
        body.SetActive(true);
        body.name = name.text;
        OrbitalBody script = body.AddComponent<OrbitalBody>();
        script.vel = new Vector3(float.Parse(xVel.text),float.Parse(yVel.text),float.Parse(zVel.text));
        script.setRadius(int.Parse(radius.text));
        script.mass = int.Parse(mass.text);
        script.type = type.options[type.value].text;
    }

}