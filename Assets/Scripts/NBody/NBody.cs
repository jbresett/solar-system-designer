using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBody : MonoBehaviour {
    public List<Body> Bodies { get; private set; }

	// Use this for initialization
	void Start () {
        Bodies = new List<Body>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
