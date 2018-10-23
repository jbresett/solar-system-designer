using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    class Body
    {
        public double mass { get; set; }
        public double radius { get; set; }
        public Dictionary<Element, double> composition { get; set; }

        public double getSurfaceGravity
        {
            get
            {
                //TODO: Get from n-body Physics.
                return 0;
            }
        }
    }
}
