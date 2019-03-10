using System.Collections;
using System.Collections.Generic;

namespace Model.Util
{
    public static class UnitConverter
    {
        private static Dictionary<string,Unit> units = new Dictionary<string,Unit>();
        
        static UnitConverter()
        {
            units.Add("absolute",new Unit("Km", "Kg", 1, 1));
            units.Add("earth",new Unit("Er","Em", 6378.1, 5.972*(10^24)));
            units.Add("earth moon", new Unit("Lr","Lm",1737.1,7.34767309*(10^22)));
        }

        public static double convertDist(double val, string baseUnit, string newUnit)
        {
            return val * (units[baseUnit].DistVal / units[newUnit].DistVal);
        }
        public static double convertMass(double val, string baseUnit, string newUnit)
        {
            return val * (units[baseUnit].MassVal / units[newUnit].MassVal);
        }
    }

    struct Unit
    {
        public readonly string DistSuff;
        public readonly string MassSuff;
        public readonly double DistVal;
        public readonly double MassVal;

        public Unit(string distSuff, string massSuff, double distVal, double massVal)
        {
            DistSuff = distSuff;
            MassSuff = massSuff;
            DistVal = distVal;
            MassVal = massVal;
        }
    }
}