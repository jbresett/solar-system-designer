using System;

namespace EllipseContruction{

	public class Ellipse{

		private double majorLen;
		private double minorLen;
		private double majorCen;
		private double minorCen;


		public Ellipse(Double majorLen, Double minorLen){
			this.majorLen = majorLen;
			this.minorLen = minorLen;
			this.majorCen = majorLen/2;
			this.minorCen = minorLen/2;
		}

		public double calculateFoci(){
			double a = Math.Pow(majorCen, 2);
			double b = Math.Pow(minorCen, 2);
			double foci = Math.Sqrt((a-b));
			return foci;
		}

		public double calculateEccentricity(){
			double foci = calculateFoci();
			double aSqr = Math.Pow(minorCen,2);
			double bSqr = Math.Pow(foci, 2);
			double cSqr = aSqr + bSqr;
			double c = Math.Sqrt(cSqr);
			double e = foci/c;

			return e;
		}

		public static void Main(String[] args){
			String majorStr;
			double major;
			String minorStr;
			double minor;
			Ellipse ellipse;
			double foci;
			double e;

			Console.WriteLine("What is the major length of your ellipse?: ");
			majorStr = Console.ReadLine();

			Console.WriteLine("\nWhat is the minor length of your ellipse?: ");
			minorStr = Console.ReadLine();

			major = double.Parse(majorStr);
			minor = double.Parse(minorStr);
			ellipse = new Ellipse(major,minor);

			foci = ellipse.calculateFoci();
			e = ellipse.calculateEccentricity();

			Console.WriteLine("\nYour Foci are " + foci + " from the center!");
			Console.WriteLine("Your eccentricity is " + e);

		}

	}
}