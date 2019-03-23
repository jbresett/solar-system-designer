namespace Model.Util
{
    public struct Unit
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