/// <summary>
/// This class utilizes the laws of orbital mechanics
/// to simulate orbit based on a planet and a stars mass
/// along with their distance from eachother.
///
/// The formulas for this class were obtained from
/// https://evgenii.com/blog/earth-orbit-simulation
///
/// @author Jack Northcutt
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity{

	private const double g = 6.67408e-11;
	private const double time = 100;

	//mass
	private double stillBody;
	private double moveBody;

	//distance and angle
	private double distance;
	private double angle;

	//current distance of the planet from sun
	private double currentDistance;

	//current velocity at which a planet travels around the star
	private double currentDistVel;

	//the current angle of planet from the sun
	private double currentAngle;

	//the current angular velocity
	private double currentAngularVel;

	/// <summary>
	/// constructor will intialize values and convert to Km using
	/// constants defined.
	/// </summary>
	public Gravity(double stillBody, double moveBody, double baryDist){

		this.stillBody = stillBody;
		this.moveBody = moveBody;
		this.distance = baryDist;
		this.currentDistance = distance;
		this.currentDistVel = 0;
		this.currentAngle = System.Math.PI/2;
		this.currentAngularVel = 0;
	}

	/// <summary>
	/// partial derivative of the angle of the Lagrangian equation
	/// L = (m/2)* ((r^2) + (r^2)(theta^2)) + (GMm)/r
	/// with respect to the anglet theta. Derived becomes
	/// distance = rDist * thetaVelocity^2 - GM/rDist^2
	/// </summary>
	public double calcDist(){

		double rThetaSqr = 0.0;
		double gMassRSqr = 0.0;
		double val = 0.0;
		rThetaSqr = currentDistance * System.Math.Pow(currentAngularVel, 2);
		gMassRSqr = (g * stillBody)/ System.Math.Pow(currentDistance, 2);
		val = rThetaSqr - gMassRSqr;

		return val;
	}

	/// <summary>
	/// partial derivative of the angle of the Lagrangian equation
	/// L = (m/2)* ((r^2) + (r^2)(theta^2)) + (GMm)/r
	/// with respect to the anglet theta. Derived becomes
	/// angle = -(2*(rVelocity)*(angularVelocity)/rDistance
	/// </summary>
	public double calcAngle(){

		double val = 0.0;
		val = (currentDistVel * currentAngularVel * (-2))/ currentDistance;

		return val;
	}

	/// <summary>
	/// Simple method to convert polar coordinates to cartesian
	/// </summary>
	public Vector3d convertToCartesian(double r, double theta){

		double x = r * System.Math.Cos(theta);

		double y = r * System.Math.Sin(theta);

		double z = 0.0;

		Vector3d coords = new Vector3d(x,y,z);

		return coords;

	}

	/// <summary>
	/// This method will run the calculations to create new
	/// x, y, z positions of the planet as it orbits around the
	/// sun.
	/// </summary>
	public Vector3d calcPosition(){

		Vector3d coords;

		currentDistVel = currentDistVel + time * calcDist();
		currentDistance = currentDistance + time * currentDistVel;

		currentAngularVel = currentAngularVel + time * calcAngle();
		currentAngle = currentAngle + time * currentAngularVel;

		coords = convertToCartesian(currentDistance, currentAngle);

		return coords;
	}


}
