using System;
using System.Collections;
using System.Collections.Generic;
using Util;

namespace Model.Util
{
    public static class UnitConverter
    {
        // Astronomical Units to Earth's Radius.
        public static readonly Dictionary<UnitType,Unit> units = new Dictionary<UnitType,Unit>();
        public static readonly Dictionary<String,UnitType> unitTypes = new Dictionary<string,UnitType>();
        
        //Converts Units Between Kilometers, Earths, Earth Moons and Suns
        static UnitConverter()
        {
            units.Add(UnitType.Absolute,new Unit("Km", "Kg", 1, 1));
            units.Add(UnitType.Earths,new Unit("Er","Em", 6378.1, 5.972*Math.Pow(10,24)));
            units.Add(UnitType.EarthMoons, new Unit("Lr","Lm",1737.1, 7.34767309*Math.Pow(10,22)));
            units.Add(UnitType.Suns,new Unit("Sr","Sm",696342,1.9885*Math.Pow(10,30)));
            
            unitTypes.Add("absolute",UnitType.Absolute);
            unitTypes.Add("earths",UnitType.Earths);
            unitTypes.Add("earth moons",UnitType.EarthMoons);
            unitTypes.Add("suns",UnitType.Suns);
        }

        public static double convertRadius(double val, UnitType baseUnit, UnitType newUnit)
        {
            return val * (units[baseUnit].DistVal / units[newUnit].DistVal);
        }
        public static double convertMass(double val, UnitType baseUnit, UnitType newUnit)
        {
            return val * (units[baseUnit].MassVal / units[newUnit].MassVal);
        }
    }
}