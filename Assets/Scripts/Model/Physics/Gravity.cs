/// <summary>
/// This class utilizes the laws of orbital mechanics
/// to simulate orbit based on a planet and a stars mass
/// along with their distance from eachother.
///
/// The formulas for this class were obtained from
/// https://www.wired.com/2016/06/way-solve-three-body-problem/
/// https://fiftyexamples.readthedocs.io/en/latest/gravity.html
/// @author Jack Northcutt
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour{

	private const double g = 6.67408E-11;
	//private const double time = 86400; //seconds in a day
	/// <summary>
	/// This method checks for the body with the most mass and keeps the
	/// velocity for the most mass at zero so that the solar system stays relative
	/// The method then checks each body to see if it has an inital velocity yet or not
	/// and then calculates that bodies initial velocity.
	///
	/// This method may not account for binary star systems and may not represent the actual starting
	/// velocity of a planet. This method calculates a guess based on mass and distance.
	///
	/// Partial Equation derrived from:
	/// https://www.physicsclassroom.com/class/circles/Lesson-4/Mathematics-of-Satellite-Motion
	/// </summary>
	public void calcInitialVelocities()
	{
		List<Body> bodyList = Sim.Bodies.Active;
		Body mostPull = new Body();
		double xDist = 0;
		Vector3d dist = new Vector3d();
		Vector3d vec = new Vector3d(0,0,0);
		int numStars = 0;
		// checking distance and calculating.
		foreach (Body body in bodyList)
		{
			if (body.Type == BodyType.Star)
			{
				numStars++;
			}
		}
		
		foreach (Body body in bodyList)
		{
			mostPull = body.MostPull;
			if (!body.isInitialVel)
			{
				if (numStars >= 2)
				{
					dist = mostPull.Pos - body.Pos;
					xDist = dist.magnitude;
					vec.z = (Math.Sqrt((g * (mostPull.KG)) / xDist));
					body.Vel = vec;
					body.isInitialVel = true;
				}
				else if (body.Type != BodyType.Star)
				{
					dist = mostPull.Pos - body.Pos;
					xDist = dist.magnitude;
					vec.z = (Math.Sqrt((g * (mostPull.KG)) / xDist));
					body.Vel = vec;
					body.isInitialVel = true;
				}
				
			}
		}
	}
	
	/// <summary>
	/// This method calculates the force of another body on a body
	/// The equation first calculates the the distance between.
	/// Then takes the magnitude of the distance
	/// next force is calculated G * M1 * M2 / (magnitude)^2
	/// we then find the angle theta
	///
	/// we then use angle theta to calc the direction of the force
	/// </summary>
	/// <param name="body"></param>
	/// <param name="obody"></param>
	/// <returns></returns>
	public Vector3d calcForce(Body body, Body obody)
	{
		Vector3d force;
		Vector3d distance = obody.Pos - body.Pos;
		double distMag = distance.magnitude;

		double aForce = (g * body.KG * obody.KG) / (distMag * distMag);

		double theta = Math.Atan2(distance.z, distance.x);
		
		force = new Vector3d(Math.Cos(theta)*aForce, 0, Math.Sin(theta)*aForce);

		return force;
	}

	
	/// <summary>
	/// This method utilizes the above forceCalc method to
	/// sum the force applied on each body by each body.
	/// </summary>
	public void updateForce()
	{
		List<Body> bodies = Sim.Bodies.Active;
		Vector3d force;
		Vector3d aforce = new Vector3d();
		double mostForce = 0;
		Body mostPull = new Body();
		
		foreach (Body bod in bodies)
		{
			force = new Vector3d(0,0,0);
			foreach (Body obod in bodies)
			{
				if (bod.Id != obod.Id)
				{
					aforce = calcForce(bod, obod);
					if (aforce.magnitude > mostForce)
					{
						mostForce = aforce.magnitude;
						mostPull = obod;

					}
					force += aforce;
				}

				bod.MostPull = mostPull;
			}

			bod.totalForce = force;
		}
	}

	/// <summary>
	///  This method uses a bodies total force to calculate a new velocity
	/// This function calculates new velocity for each body.
	/// </summary>
	public void updateVelocity()
	{
		updateForce();
		calcInitialVelocities();
		List<Body> bodies = Sim.Bodies.Active;
		Vector3d velocity;
		foreach (Body bod in bodies)
		{
			velocity = bod.Vel;
			velocity.x += bod.totalForce.x / bod.KG * Sim.Settings.Speed;
			velocity.z += bod.totalForce.z / bod.KG * Sim.Settings.Speed;

			bod.Vel = velocity;
		}
	}
	

	/// <summary>
	/// THis function uses the new velocities to calculate a new position
	/// </summary>
	public void calcPosition()
	{
		updateVelocity();
		List<Body> bodies = Sim.Bodies.Active;
		Vector3d newPos;
		foreach (Body bod in bodies)
		{
			newPos = new Vector3d(bod.Pos.x + bod.Vel.x * Sim.Settings.Speed, 0,bod.Pos.z + bod.Vel.z * Sim.Settings.Speed);
			bod.Pos = newPos;
		}
	}

	// Update is called once per frame
	public void Update () {
        if (!Sim.Settings.Paused)
        { 
            calcPosition();
        }
    }

}
