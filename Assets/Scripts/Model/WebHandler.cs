using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interactions with the Web-Server (http and JavaScript).
/// </summary>
public class WebHandler : Singleton<WebHandler> {

    // Invalid Parameeter Message. {Parameter}.
    private const string INVALID_PARAM_MSG = "Invalid Parameter: {0} is not a 'key=value' pair.";

    // URL Parameters.
    Dictionary<string, string> parameters = new Dictionary<string, string>();
    public Dictionary<string, string> Param { get { return parameters; } }

    /// <summary>
    /// Retrives any URL parameters on initiation.
    /// </summary>
    public void Init() {

        // Get Application URL
        string url = Application.absoluteURL;
        
        // Return if there are no parameters to add.
        if (url.IndexOf("?") == -1) return;

        // Get everything after the '?'.
        string paramStr = url.Split('?')[1];

        // Convert Parameters into dictionary.
        foreach (string param in paramStr.Split('&'))
        {
            // 'Key=Value' pair.
            string[] pair = param.Split('=');

            // Check for proper array length:
            // * must have exactly 1 '='
            // * include both a key and value (neither 0-length).
            if (pair.Length == 2 && pair[0].Length > 0 && pair[1].Length > 0)
            {
                // Convert from escaped URL to standard string.
                parameters.Add(WWW.UnEscapeURL(pair[0]), WWW.UnEscapeURL(pair[1]));
            }
            // Ignore incorrect array length (non 'key=value' string).
            else
            {
                Debug.Log(string.Format(INVALID_PARAM_MSG, param));
            }
        }

    }

    /// <summary>
    /// Generates a URL based on the current URL and a dictionary of paremeters.
    /// 
    /// Used to turn an solar systems generated within the Sim to a URL format.
    /// </summary>
    /// <param name="dictionary">dictionary to use. If null or not set, the current dictionary will be used.</param>
    /// <returns></returns>
    public string ToUrl(Dictionary<string, string> dictionary = null)
    {
        // Use default if null.
        if (dictionary == null) dictionary = parameters;

        // Base URL.
        string url = Application.absoluteURL.Split('?')[0] + "?";
        
        // Add each parameters.
        foreach(KeyValuePair<string, string> pair in dictionary)
        {
            url += WWW.EscapeURL(pair.Key) + "&" + WWW.EscapeURL(pair.Value);
        }

        return url;
    }

}
