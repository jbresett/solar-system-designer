using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sim : MonoBehaviour {

    /// <summary>
    /// Main Simulation Initiation
    /// </summary>
    private void Awake()
    {
        Perm.Awake();
        Capi.Awake();
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
