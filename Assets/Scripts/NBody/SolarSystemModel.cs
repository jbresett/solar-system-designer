/** 
 *This class will be used to load a model of a solar
 *System so that we can quickly add a solar system
 *when testing.
 *
 *@author Jack Northcutt
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SolarSystemModel{

	[SerializeField]
	private NBody system;
	
	[SerializeField]
	private Body[] systemBodies;

	public SolarSystemModel(){
		system = new NBody();
		systemBodies = new Body[5];
	}

	//This method will load the array of bodies, so that we
	// can easily add them to our simulation scene
	public void loadModel(){
		// Body test = new Body();
		// Body sun = new Body(system, "Sun", test.BodyType.sun=Sun, null, 1000, 1000, null);
		// Body planet1 = new Body(system, "Earth", Planet, null, 500, 500, null);
		// Body planet2 = new Body(system, "Mars", Planet, null, 500, 500, null);
		// Body planet3 = new Body(system, "Jupiter", Planet, null, 500, 500, null);
		// Body planet4 = new Body(system, "Saturn", Planet, null, 500, 500, null);
	}
}
