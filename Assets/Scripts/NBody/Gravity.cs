using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity{

	private const double earthMass = 5.972 * System.Math.Pow(10, 24);
	private const double sunMass = 1.989 * System.Math.Pow(10,30);
	private const double au = 1.496 * System.Math.Pow(10,8);
	private const double g = 6.67408 * System.Math.Pow(10, -11);
	private const double time = 100;

	private double starMass;
	private double planetMass;
	private double distance;
	private double angle;
	private double currentDistance;
	private double currentDistDeriv;
	private double currentAngle;
	private double currentAngleDeriv;

	public Gravity(double starMass, double planetMass, double dist){
		this.starMass = starMass * sunMass;
		this.planetMass = planetMass * earthMass;
		this.distance = dist *au;
		this.currentDistance = distance;
		this.currentDistDeriv = 0;
		this.currentAngle = System.Math.PI/2;
		this.currentAngleDeriv = 0;
	}

	public double calcDist(){
		return 0;
	}

	public double calcAngle(){
		return 0;
	}


}
