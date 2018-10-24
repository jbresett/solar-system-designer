using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    class Layer
    {

        /// <summary>
        /// Creates a layer with no size or composition.
        /// </summary>
        /// <param name="name"></param>
        public Layer(String name) : this(name, 0.0, 0.0) { }

        /// <summary>
        /// Creates a layer with unknown composition.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromRadius"></param>
        /// <param name="toRadius"></param>
        public Layer(String name, double fromRadius, double toRadius) : this(name, fromRadius, toRadius, null) { }

        /// <summary>
        /// Creates a layer.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fromRadius"></param>
        /// <param name="toRadius"></param>
        /// <param name="composition"></param>
        public Layer(String name, double fromRadius, double toRadius, Dictionary<Compound, double> composition)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            this.Name = name;
            this.FromRadius = fromRadius;
            this.ToRadius = toRadius;
            if (composition != null)
            {
                this.Composition = new Dictionary<Compound, double>(composition);
            }
        }

        public string Name { get; set;  }
        /// <summary>
        /// Inner Radius
        /// </summary>
        public double FromRadius { get; set; }
        /// <summary>
        /// Outer radius.
        /// </summary>
        public double ToRadius { get; set; }
        public Dictionary<Compound, double> Composition { get; }

    }
}
