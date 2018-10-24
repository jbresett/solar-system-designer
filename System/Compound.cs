using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Simplified Compound Structure (by Formula).  If more details are needed later
/// in the project, this can be upgraded to handle by element structures and
/// verification of Hill-System order.
/// </summary>
namespace System
{
    class Compound
    {

        /// <summary>
        /// Creates a compound based on a formula. Hill-System should be used
        /// for element order.
        /// </summary>
        /// <param name="formula"></param>
        public Compound(String formula)
        {
            if (string.IsNullOrEmpty(formula))
            {
                throw new ArgumentException("message", nameof(formula));
            }

            this.Formula = formula;
        }
 
        string Formula { get; }
    }
}
