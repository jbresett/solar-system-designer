/**
 * This Class tests out a formual to find the time it takes
 * a planet ino our soloar system to orbit the sun, when given
 * a distance from the sun
 * 
 * @author Jack Northcutt
 */

using System;
namespace ThirdLawTest
{
	class ThirdLaw
	{
		static void Main()
		{
			Console.WriteLine("Earth is 1 AU from the sun");
			Console.WriteLine("Enter your planets distance from the sun in AU: ");
			string au;
			double auDoub;
			double days;
			double distance;
			double years;
			au = Console.ReadLine();
			auDoub = double.Parse(au);
			distance = Math.Pow(auDoub,3);
			years = Math.Sqrt(distance);
			days = years *365;
			Console.WriteLine("\nYour Planent will take " + days + " to orbit the sun!");
		}
	}
}