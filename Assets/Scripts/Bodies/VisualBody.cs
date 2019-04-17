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
            if (Application.isPlaying) return; // position updated via Update() in play mode.
            transform.position = value.Vec3 * POSITION_MULT;
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
    
    /// <summary>
    /// This function is so that we can determine
    /// which body is currently selected.
    /// </summary>
    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }  
    }

    [SerializeField]
    protected bool isSelected;
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

        // Update Rotation
        if (!Sim.Settings.Paused)
        {
            transform.Rotate((Vector3.up * -(float)Rotation) * (Time.deltaTime * (float)Sim.Settings.Speed), Space.Self);
        }
        Debug.Log(Position.Vec3 + " ... " + POSITION_MULT + "..." + transform.position);
        Debug.Log(Position.Vec3 * POSITION_MULT);

        Vector3 move = (Position.Vec3 * POSITION_MULT) - transform.position;
        transform.position += move;

    }
}
