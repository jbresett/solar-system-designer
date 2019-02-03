/// <summary>
/// This class utilizes the laws of orbital mechanics
/// to simulate orbit based on a planet and a stars mass
/// along with their distance from eachother.
///
/// The formulas for this class were obtained from
/// https://evgenii.com/blog/earth-orbit-simulation
/// https://www.wired.com/2016/06/way-solve-three-body-problem/
/// @author Jack Northcutt
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity{

	private const double g = -6.67408e-11;
	private const double time = 100;

	private Bodies bodyArray;

//	//mass
//	private double stillBody;
//	private double moveBody;
//
//	//distance and angle
//	private double distance;
//	private double angle;
//
//	//current distance of the planet from sun
//	private double currentDistance;
//
//	//current velocity at which a planet travels around the star
//	private double currentDistVel;
//
//	//the current angle of planet from the sun
//	private double currentAngle;
//
//	//the current angular velocity
//	private double currentAngularVel;
//
//	/// <summary>
//	/// constructor will intialize values and convert to Km using
//	/// constants defined.
//	/// </summary>
	public Gravity()
	{

	}
	
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
		double years = 0;
		double days = 0;
		double kmSec = 0;
		Body sun = new Body();
		Vector3d initialVel;
		Vector3d baryDistVec;
		double dist;
		List<Body> bodyList = Bodies.getActive();
		
		// looking for the star
		foreach (Body body in bodyList)
		{
			if (body.Type == BodyType.Star)
			{
				sun = body;
			}
		}
		
		// checking distance and calculating.
		foreach (Body body in bodyList)
		{
			if (body.Type != BodyType.Star)
			{
				baryDistVec = body.GetBaycenter(sun);
				dist = baryDistVec[2];
				dist = dist * dist * dist;
				years = Math.Sqrt(dist);
				days = years * 360;
				kmSec = (dist * 2 * Math.PI) / (days * 24 * 60 * 60);
				body.initialVelocity = new Vector3d(0,kmSec,0);
			}
			//else for case of star
			else
			{
				body.initialVelocity = new Vector3d(0,0,0);
			}
		}
	}
	
	/// <summary>
	/// This method calculates force applied by each body to
	/// each body in the nbody system.
	/// </summary>
	/// <returns></returns>
	public double[] distCalulation()
	{
		List<Body> bodyList = Bodies.getActive();

		foreach (Body body in bodyList)
		{
			
		}
		return null;
	}
	
	/// <summary>
	/// This method uses the distance calculation in order
	/// to calculate the force applied to the nbodies
	/// </summary>
	/// <returns></returns>
	public Vector3d calculateForce()
	{
		List<Body> bodyList = Bodies.getActive();
		int numBodies = bodyList.Count;
		Vector3d forceApplied = new Vector3d();;
		Vector3d positionDiff;
		double combinedMass = 0;
 
		for (int i = 0; i < numBodies-1; i++)
		{
            //Debugger.log("Comparing " + i + " " + bodyList[i].name + bodyList[i].Position + " " + bodyList[i+1].name + bodyList[i + 1].Position);
			positionDiff = bodyList[i].Position - bodyList[i+1].Position;
			combinedMass = g * bodyList[i].Mass * bodyList[i + 1].Mass / positionDiff.magnitude * positionDiff.magnitude;

			forceApplied += positionDiff.normalized * combinedMass;
		}

		return forceApplied;
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
				body.momentumVector = body.initialVelocity * body.Mass;
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
		List<Body> bodyList = Bodies.getActive();
		
		updateMomentum(calculateForce());
		foreach (Body body in bodyList)
		{
			body.Position = (body.Position + body.momentumVector) / body.Mass;
		}
	}
	
//
//	/// <summary>
//	/// partial derivative of the angle of the Lagrangian equation
//	/// L = (m/2)* ((r^2) + (r^2)(theta^2)) + (GMm)/r
//	/// with respect to the angle theta. Derived becomes
//	/// distance = rDist * thetaVelocity^2 - GM/rDist^2
//	/// </summary>
//	public double calcDist(){
//
//		double rThetaSqr = 0.0;
//		double gMassRSqr = 0.0;
//		double val = 0.0;
//		rThetaSqr = currentDistance * System.Math.Pow(currentAngularVel, 2);
//		gMassRSqr = (g * stillBody)/ System.Math.Pow(currentDistance, 2);
//		val = rThetaSqr - gMassRSqr;
//
//		return val;
//	}
//
//	/// <summary>
//	/// partial derivative of the angle of the Lagrangian equation
//	/// L = (m/2)* ((r^2) + (r^2)(theta^2)) + (GMm)/r
//	/// with respect to the anglet theta. Derived becomes
//	/// angle = -(2*(rVelocity)*(angularVelocity)/rDistance
//	/// </summary>
//	public double calcAngle(){
//
//		double val = 0.0;
//		val = (currentDistVel * currentAngularVel * (-2))/ currentDistance;
//
//		return val;
//	}
//
//	/// <summary>
//	/// Simple method to convert polar coordinates to cartesian
//	/// </summary>
//	public Vector3d convertToCartesian(double r, double theta){
//
//		double x = r * System.Math.Cos(theta);
//
//		double y = r * System.Math.Sin(theta);
//
//		double z = 0.0;
//
//		Vector3d coords = new Vector3d(x,y,z);
//
//		return coords;
//
//	}
//
//	/// <summary>
//	/// This method will run the calculations to create new
//	/// x, y, z positions of the planet as it orbits around the
//	/// sun.
//	/// </summary>
//	public Vector3d calcPosition(){
//
//		Vector3d coords;
//
//		currentDistVel = currentDistVel + time * calcDist();
//		currentDistance = currentDistance + time * currentDistVel;
//
//		currentAngularVel = currentAngularVel + time * calcAngle();
//		currentAngle = currentAngle + time * currentAngularVel;
//
//		coords = convertToCartesian(currentDistance, currentAngle);
//
//		return coords;
//	}


}
