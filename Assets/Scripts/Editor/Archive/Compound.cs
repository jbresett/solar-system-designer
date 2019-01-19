using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Compound
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
            throw new ArgumentException("Formula is null.", formula);
        }

        this.Formula = formula;
    }
 
    public string Formula { get; private set; }
}
