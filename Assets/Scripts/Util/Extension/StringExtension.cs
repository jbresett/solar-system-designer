using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Extension Utilities.
public static class StringExtension
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

    /// <summary>
    /// Returns the Enum value based on the string. 
    /// Spaces will be replaced with underscores.
    /// All other non alphanumeric characters will be removed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    public static T Enum<T>(this string input)
    {
        // Remove parenthesis, Replace spaces with underscores.
        input = Regex.Replace(input.Replace(" ", "_"), "[^A-Za-z0-9_]", "");
        return (T)System.Enum.Parse(typeof(T), input);
    }

    public static string IfBlank(this string input, string alternate)
    {
        return string.IsNullOrEmpty(input) ? alternate : input;
    }

}


