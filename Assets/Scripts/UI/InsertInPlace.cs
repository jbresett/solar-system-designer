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
    public Mesh sphereMesh;

    void Start()
    {
        button.onClick.AddListener(insert);
    }
    public void insert()
    {
        Debug.Log("Inserting Object");
        GameObject body = new GameObject(name.text);
        Vector3 pos = new Vector3(float.Parse(xPos.text),float.Parse(yPos.text),float.Parse(zPos.text));
        Quaternion rot = new Quaternion(0,0,0,0);
        body.AddComponent<MeshFilter>();
        var mesh= body.GetComponent<MeshFilter>().mesh;
        mesh = sphereMesh;
        body.GetComponent<MeshFilter>().mesh = mesh;
        body.AddComponent<MeshRenderer>();
        Instantiate(body, pos, rot);
    }
}