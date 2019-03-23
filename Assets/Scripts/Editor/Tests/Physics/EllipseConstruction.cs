/// <summary>
///	This file constructs an ellipse based off of the major and minor axis
/// The class then calculates an where the foci are positioned, which will
/// represent where the sun will be placed in the ellipse and determines the
/// eccentricity of the ellipse. THe eccentricity determines the elongation 
/// of the ellipse and is used to to calculate the distance a planet is from
/// the foci at any given time. THis class contains a basic tester method
/// to make sure calculations are correct.
	
/// This file now calculates distance from panet and sun(foci) and also
/// can determine the position of a point on an ellipse


/// @author Jack Northcutt
/// </summary>
using System;

namespace EllipseContruction{

	public class Ellipse{

		private double majorLen;
		private double minorLen;
		private double majorCen;
		private double minorCen;
		private double eccen;

		/// <summary>
		/// constructor for ellipse
		/// </summary>
		public Ellipse(Double majorLen, Double minorLen){
			this.majorLen = majorLen;
			this.minorLen = minorLen;
			this.majorCen = majorLen/2;
			this.minorCen = minorLen/2;
		}

		/// <summary>
		/// determines foci points on ellipse
		/// </summary>
		public double calculateFoci(){
			double a = Math.Pow(majorCen, 2);
			double b = Math.Pow(minorCen, 2);
			double foci = Math.Sqrt((a-b));
			return foci;
		}

		/// <summary>
		/// calculates the eccentricity of an ellipse
		/// </summary>
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

		/// <summary>
		/// calculates the distance of a point on the ellipse
		/// to the foci of an ellipse
		/// </summary>
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

		/// <summary>
		/// Takes the 'y' position of a point on an ellipse and
		/// calculates the the x position
		/// <summary>
		public double calculatePosition(double x){

			//set up a b x and y values
			double y = 0;
			double a = 0;
			double b = 0;

			a = majorLen;
			b = minorLen;

			//squrar each of the values
			a= Math.Pow(a, 2);
			b = Math.Pow(b, 2);
			x = Math.Pow(x, 2);

			//use equaltion (x^2)/a^2 + (y^2)/b^2 = 1
			y = Math.Sqrt((1 - (x/a))*b);

			//return the new why value and combined with
			//x with give you the coordinates
			return y;
		}

		/// <summary>
		/// main method to run command line test to see if
		/// correct values are displayed when given know 
		/// values
		/// </summary>
		public static void Main(String[] args){

			//Set up variables used
			String majorStr;
			double major;
			String minorStr;
			double minor;
			Ellipse ellipse;
			double foci;
			double e;
			String angleStr;
			double angle;
			String xStr;
			double x;

			//Get input from user
			Console.WriteLine("What is the major length of your ellipse?: ");
			majorStr = Console.ReadLine();

			Console.WriteLine("\nWhat is the minor length of your ellipse?: ");
			minorStr = Console.ReadLine();

			Console.WriteLine("\nWhat is the angle of your planet from sun?: ");
			angleStr = Console.ReadLine();

			Console.WriteLine("\nWhat is your x position?: ");
			xStr = Console.ReadLine();

			//convert user input to doubles
			major = double.Parse(majorStr);
			minor = double.Parse(minorStr);
			angle = double.Parse(angleStr);
			x = double.Parse(xStr);

			//create ellipse object and set foci and e
			ellipse = new Ellipse(major,minor);
			foci = ellipse.calculateFoci();
			e = ellipse.calculateEccentricity();

			//dispay results to user
			Console.WriteLine("\nYour Foci are " + foci + " from the center!");

			Console.WriteLine("Your eccentricity is " + e);	

			Console.WriteLine("Your planet is " + ellipse.calculateDistance(angle) + (" from the sun!"));

			Console.WriteLine("Your Planets Position is: " + x + "," + ellipse.calculatePosition(x));
		}

	}
}