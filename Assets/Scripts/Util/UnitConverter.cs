using System;
using System.Collections;
using System.Collections.Generic;

namespace Model.Util
{
    public static class UnitConverter
    {
        // Astronomical Units to Earth's Radius.


        public static readonly Dictionary<string,Unit> units = new Dictionary<string,Unit>();
        
        static UnitConverter()
        {
            units.Add("absolute",new Unit("Km", "Kg", 1, 1));
            units.Add("earths",new Unit("Er","Em", 6378.1, 5.972*Math.Pow(10,24)));
            units.Add("earth moons", new Unit("Lr","Lm",1737.1, 7.34767309*Math.Pow(10,22)));
            units.Add("suns",new Unit("Sr","Sm",696342,1.9885*Math.Pow(10,30)));
        }

        public static double convertRadius(double val, string baseUnit, string newUnit)
        {
            return val * (units[baseUnit].DistVal / units[newUnit].DistVal);
        }
        public static double convertMass(double val, string baseUnit, string newUnit)
        {
            return val * (units[baseUnit].MassVal / units[newUnit].MassVal);
        }
    }
}