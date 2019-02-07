using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sim : MonoBehaviour {

    // Location for the Active and Inactive body containers.
    public GameObject BodyContainer;

    public GameObject BodyPrefab;

    /// <summary>
    /// Main Simulation Initiation
    /// </summary>
    private void Awake()
    {
        Perm.Init();
        Bodies.Init(BodyContainer, BodyPrefab);
        Capi.Init();
        CapiBody.Init();
        
    }

    void Start () {
        Perm.Start();
        Capi.Start();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
