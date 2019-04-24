using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Extension Utility for the String class.
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

    /// <summary>
    /// Returns the alternate value if the first is null or blank.
    /// </summary>
    /// <param name="primary">Primary value.</param>
    /// <param name="alternate">Alternate value. Null or blank alternates will still be returned.</param>
    /// <returns></returns>
    public static string IfBlank(this string primary, string alternate)
    {
        return string.IsNullOrEmpty(primary) ? alternate : primary;
    }

    /// <summary>
    /// Converts normal text to a URL friendly escape format.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string Escape(this string input)
    {
        return WWW.EscapeURL(input);
    }

    /// <summary>
    /// Converts a URL friendly escape format back to normal text.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string UnEscape(this string input)
    {
        return WWW.UnEscapeURL(input);
    }

}


