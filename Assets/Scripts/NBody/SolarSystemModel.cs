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
[Obsolete]
/// Disabled per Sol System system loaded at default through Unity Scenes.
public class SolarSystemModel{

	[SerializeField]
	private NBody system;

	public SolarSystemModel(){
		system = new NBody();
		loadDefault();
	}

 	//This method returns the NBody system, which should contain a defualt
 	//dictionary filled with our solar system.
	public NBody getModel(){
		return system;
	}

	//This method loads an NBody system with a default list of bodies
	//currently this method contains bodies that represent our solar system
	//the bodies will be loaded into our system and values will be tweaked as
	//needed
	public void loadDefault(){
        /* Disabled per Sol System system loaded at default through Unity Scenes.
		Body sun = new Body(system, "Sun", BodyType.Sun, null, 500, 500, 500);
		system.Add("Sun",sun);
		Body mercury = new Body(system, "Mercury", BodyType.Planet, null, 500, 500, 500);
		system.Add("Mercury",mercury);
		Body venus = new Body(system, "Venus", BodyType.Planet, null, 500, 500, 500);
		system.Add("Venus",venus);
		Body earth = new Body(system, "Earth", BodyType.Planet, null, 500, 500, 500);
		system.Add("Earth", earth);
		Body mars = new Body(system, "Mars",  BodyType.Planet, null, 500, 500, 500); 
		system.Add("Mars", mars);
		Body jupiter = new Body(system, "Jupiter", BodyType.Planet, null, 500, 500, 500);
		system.Add("Jupiter",jupiter);
		Body saturn = new Body(system, "Saturn",  BodyType.Planet, null, 500, 500, 500); 
		system.Add("Saturn",saturn);
		Body neptune = new Body(system, "Neptune", BodyType.Planet, null, 500, 500, 500);
		system.Add("Neptune",neptune);
		Body earthMoon = new Body(system, "Earth Moon", BodyType.Moon, null, 500, 500, 500);
		system.Add("Earth Moon", earthMoon);
        */
	}

	//this method will be used to add additional bodies to the
	//list of bodies if we want to further testing.
	public void addBody(Body body){
		//string name = body.Name;
		//system.Add(name, body);
	}

	//this method will be used to remove a body from
	//the system.
	public void removeModel(Body body){
		string name = body.Name;

		if (system.ContainsKey(name)){
			system.Remove(name);
		}
		else{
			throw new InvalidOperationException("Body is not contained in the current N-Body system!");
		}
	}
}
