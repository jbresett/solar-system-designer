using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBody : BaseBody {

    // Multiplers for standard Earths -> Unity
    static public float POSITION_MULT = 30492.3f;
    static public float SIZE_MULT = .1f;     // 1 Earth = ~130 Unity units. 
    


    
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
            Material mat = (Material)Resources.Load("Body/" + value.ToString(), typeof(Material));
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

        Vector3 move = (Position.Vec3 * POSITION_MULT) - transform.position;
        transform.position += move;
        if (Sim.selectedBody != null)
        {
            Vector3 offset = Sim.selectedBody.Position.Vec3 * POSITION_MULT;
            transform.position -= offset;
        }
        setBodyEffects();
    }

    private void setBodyEffects()
    {
        
        double sizeFactor = (diameter / (position.Vec3 - Camera.main.transform.position).magnitude);
        if (type == BodyType.Star)
        {
            light.enabled = true;
            sunLighting.SetActive(true);
        }
        else
        {
            GetComponent<Renderer>().material.SetColor("_EmissionColor",Color.white*(float)(.0001/Math.Pow(sizeFactor,2)));
            light.enabled = false;
            sunLighting.SetActive(false);
        }
    }
}
