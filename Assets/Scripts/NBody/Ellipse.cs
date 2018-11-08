/** 
 * This Class is used for the creation of an Ellipse object
 * This class contains setters and getter methods for an
 * ellipse object.
 *
 *@author Jack Northcutt
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellipse{

	private double majorAxis;
	private double minorAxis;

	//constructor
	public Ellipse(double major, double minor){
		this.majorAxis = major;
		this.minorAxis = minor;
	}

	//major axis setter
	public void setMajorAxis(double major){
		this.majorAxis = major;
	}

	//minor axis setter
	public void setMinorAxis(double minor){
		this.minorAxis = minor;
	}

	//major axis getter;
	public double getMajorAxis(){
		return this.majorAxis;
	}

	//minor axis getter
	public double getMinorAxis(){
		return this.minorAxis;
	}

	//this method yields a double value that represents
	//how far the foci are offset from the center of
	//the ellipse. The foci is where the sun will be placed.
	public double calcFoci(){

		double a = System.Math.Pow(this.majorAxis/2, 2);
		double b = System.Math.Pow(this.minorAxis/2, 2);

		double foci = System.Math.Sqrt(a-b);

		return foci;
	}

	//This function calculates the eccentricity of an ellipse,
	//which is the elongation ratio of an ellipse. The closer
	//to zero the more circular the ellipse is and the closer
	//to 1 the more elongated the ellipse is. 
	public double calcEccentricity(){

		double foci = calcFoci();
		double aSqr = System.Math.Pow(minorAxis/2, 2);
		double bSqr = System.Math.Pow(foci, 2);
		double cSqr = aSqr + bSqr;
		double c = System.Math.Sqrt(cSqr);
		double e = foci/c;

		return e;
	}

	//This Function takes a double angle and calculates 
	//the x y and z position of where the point lies 
	//on the ellipse. THis function returns a Vector3D
	public Vector3d calcPoint(double theta){

		//converting the angle give to radians
		theta = (theta * System.Math.PI)/ 180;

		//calculating x y and defaulting z to zero
		double x = (this.majorAxis/2) * System.Math.Cos(theta);

		double y = (this.minorAxis/2) * System.Math.Sin(theta);

		double z = 0.0;

		//createing a vector and returning
		Vector3d coords = new Vector3d(x,y,z);

		return coords;

	}

}
