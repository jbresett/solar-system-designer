using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{

    // Extra Utilities.
    public static class StringUtil
    {

        /// <summary>
        /// Adds .Format function to Strings.
        /// </summary>
        /// <param name="input">Initial String</param>
        /// <param name="args">Agruments</param>
        /// <returns></returns>
        public static string Format(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

    }
}

