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
	/// This method uses the distance calculation in order
	/// to calculate the force applied to the nbodies
	/// </summary>
	/// <returns></returns>
//	public Vector3d calculateForce()
//	{
//		List<Body> bodyList = Bodies.getActive();
//		int numBodies = bodyList.Count;
//		Vector3d forceApplied = new Vector3d();;
//		Vector3d positionDiff;
//		double combinedMass = 0;
// 
//		for (int i = 0; i < numBodies-1; i++)
//		{
//			for (int j = i+1; j < numBodies; j++)
//			{
//				//Debugger.log("Comparing " + i + " " + bodyList[i].name + bodyList[i].Position + " " + bodyList[i+1].name + bodyList[i + 1].Position);
//				positionDiff = bodyList[i].Pos - bodyList[j].Pos;
//				combinedMass = -g * bodyList[i].KG * bodyList[j].KG / positionDiff.magnitude *
//				               positionDiff.magnitude;
//
//				forceApplied += positionDiff.normalized * combinedMass;
//			}
//		}
//
//		return forceApplied;
//	}

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
	/// This method uses the force calculated to
	/// update the momentum of the nbodies
	/// </summary>
	public void updateMomentum(Vector3d force)
	{
		List<Body> bodyList = Bodies.getActive();
	
		foreach (Body body in bodyList)
		{
			if (body.momentumVector == null)
			{
				calcInitialVelocities();
				body.momentumVector = body.initialVelocity * body.KG;
			}
			else
			{
				body.momentumVector = body.momentumVector + force;
			}

		}
	}
	
	/// <summary>
	/// This method utilizes the momentum in order to
	/// determine the nbodies new positions.
	/// </summary>
	public void calcPosition()
	{
//		List<Body> bodyList = Bodies.getActive();
//		
//		updateMomentum(calculateForce());
//		foreach (Body body in bodyList)
//		{
//			body.Pos = (body.Pos + body.momentumVector) / body.KG;
//		}
	}

	// Update is called once per frame
	public void Update () {
		calcPosition();

	}

}
