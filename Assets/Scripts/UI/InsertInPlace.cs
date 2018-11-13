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
    public GameObject planetBase;

    void Start()
    {
        button.onClick.AddListener(insert);
    }
    public void insert()
    {
        var body = planetBase;
        Vector3 pos = new Vector3(float.Parse(xPos.text),float.Parse(yPos.text),float.Parse(zPos.text));
        Quaternion rot = new Quaternion(0,0,0,0);
        Vector3 size = body.transform.localScale;
        size.x = float.Parse(radius.text);
        size.y = float.Parse(radius.text);
        size.z = float.Parse(radius.text);
        body.transform.localScale = size;
        body.SetActive(true);
        body.name = name.text;
        Instantiate(body, pos, rot);
    }
}