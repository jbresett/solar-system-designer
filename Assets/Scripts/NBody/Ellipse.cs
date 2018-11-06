/** 
 * This Class is used for the creation of an Ellipse object
 * This class contains setters and getter methods for an
 * ellipse object.
 *
 *@author Jack Northcutt
 */
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

}
