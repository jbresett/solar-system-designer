using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBody : BaseBody {

    // Multiplers for standard Earths -> Unity
    static public float POSITION_MULT = 10f;   // 1 AU = 10 Unity units.
    static public float SIZE_MULT = 1f;     // 1 Earth = 1 Unity units.

    new public Vector3d Position {
        get { return position; }
        set
        {
            base.Position = value;
            gameObject.transform.position = value.Vec3 * POSITION_MULT;
        }
    }

    new public double Diameter
    {
        get { return diameter; }
        set
        {
            base.Diameter = value;
            float size = (float)value * SIZE_MULT;
            gameObject.transform.localScale = new Vector3(size, size, size);
        }
    }

    new public BodyMaterial Material
    {
        get { return material; }
        set
        {
            base.Material = value;
            Material mat = (Material)Resources.Load(value.ToString(), typeof(Material));
            gameObject.GetComponent<MeshRenderer>().material = mat;
        }
    }

    new public void Awake()
    {
        base.Awake();
    }

	new public void Start ()
    {
        base.Start();
	}

	new public void Update ()
    {
        base.Update();
        transform.Rotate((Vector3.up * (float)rotation) * Time.deltaTime, Space.Self);
    }
}
