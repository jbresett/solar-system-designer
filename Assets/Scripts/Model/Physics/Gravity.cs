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
	private const double time = 10000;
	/// <summary>
	/// This calculates the initial velocities of all the bodies.
	/// The function works by checking all bodies from the list
	/// for the body that is a star, then compares the distance from the
	/// sun to a given body and utilizes kepler's erd law to calculate
	/// the total years it takes to make a revolution. The function then
	/// finds the estimated circumference of the orbit and divides by the number
	/// of seconds to get the km/sec velocity
	/// </summary>
	public void calcInitialVelocities()
	{
		List<Body> bodyList = Bodies.getActive();
		// checking distance and calculating.
		foreach (Body body in bodyList)
		{

				body.initialVelocity = new Vector3d(0,0,0);
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
		List<Body> bodies = Bodies.getActive();
		Vector3d force;
		foreach (Body bod in bodies)
		{
			force = new Vector3d(0,0,0);
			foreach (Body obod in bodies)
			{
				if (bod.Id != obod.Id)
				{
					force += calcForce(bod, obod);
				}
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
		List<Body> bodies = Bodies.getActive();
		Vector3d velocity;
		foreach (Body bod in bodies)
		{
			velocity = bod.velocity;
			velocity.x += bod.totalForce.x / bod.KG * time;
			velocity.z += bod.totalForce.z / bod.KG * time;

			bod.velocity = velocity;
		}
	}
	

	/// <summary>
	/// THis function uses the new velocities to calculate a new position
	/// </summary>
	public void calcPosition()
	{
		updateVelocity();
		List<Body> bodies = Bodies.getActive();
		Vector3d newPos;
		foreach (Body bod in bodies)
		{
			newPos = new Vector3d(bod.Pos.x + bod.velocity.x *time,0,bod.Pos.z + bod.velocity.z *time);
			bod.Pos = newPos;
		}
	}

	// Update is called once per frame
	public void Update () {
		calcPosition();

	}

}
