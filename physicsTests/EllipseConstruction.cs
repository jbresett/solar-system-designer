/**
 	This file constructs an ellipse based off of the major and minor axis
 	The class then calculates an where the foci are positioned, which will
 	represent where the sun will be placed in the ellipse and determines the
 	eccentricity of the ellipse. THe eccentricity determines the elongation 
 	of the ellipse and is used to to calculate the distance a planet is from
 	the foci at any given time. THis class contains a basic tester method
 	to make sure calculations are correct.

 	@author Jack Northcutt
 */
using System;

namespace EllipseContruction{

	public class Ellipse{

		private double majorLen;
		private double minorLen;
		private double majorCen;
		private double minorCen;
		private double eccen;

		//contructor
		public Ellipse(Double majorLen, Double minorLen){
			this.majorLen = majorLen;
			this.minorLen = minorLen;
			this.majorCen = majorLen/2;
			this.minorCen = minorLen/2;
		}

		//determines foci
		public double calculateFoci(){
			double a = Math.Pow(majorCen, 2);
			double b = Math.Pow(minorCen, 2);
			double foci = Math.Sqrt((a-b));
			return foci;
		}

		//determines eccentricity
		public double calculateEccentricity(){
			double foci = calculateFoci();
			double aSqr = Math.Pow(minorCen,2);
			double bSqr = Math.Pow(foci, 2);
			double cSqr = aSqr + bSqr;
			double c = Math.Sqrt(cSqr);
			double e = foci/c;
			this.eccen = e;
			return this.eccen;
		}

		//determines distance of point from foci
		public double calculateDistance(double angle){
			double numer = 0;
			double denom = 0;
			double dist = 0;
			double e = 0;
			numer = majorCen*(1 - Math.Pow(e,2));
			denom = 1 + (e * Math.Cos(angle));
			dist = numer/denom;
			return dist;
		}

		//tester driver
		public static void Main(String[] args){
			String majorStr;
			double major;
			String minorStr;
			double minor;
			Ellipse ellipse;x
			double foci;
			double e;
			String angleStr;
			double angle;

			Console.WriteLine("What is the major length of your ellipse?: ");
			majorStr = Console.ReadLine();

			Console.WriteLine("\nWhat is the minor length of your ellipse?: ");
			minorStr = Console.ReadLine();

			Console.WriteLine("\nWhat is the angle of your planete from sun: ");
			angleStr = Console.ReadLine();

			major = double.Parse(majorStr);
			minor = double.Parse(minorStr);
			angle = double.Parse(angleStr);

			ellipse = new Ellipse(major,minor);
			foci = ellipse.calculateFoci();
			e = ellipse.calculateEccentricity();


			Console.WriteLine("\nYour Foci are " + foci + " from the center!");
			Console.WriteLine("Your eccentricity is " + e);	

			Console.WriteLine("Your planet is " + ellipse.calculateDistance(angle) + (" from the sun!"));
		}

	}
}