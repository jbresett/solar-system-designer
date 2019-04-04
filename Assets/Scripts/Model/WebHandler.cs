using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// Handles interactions with the Web-Server (http and JavaScript).
/// 
/// Note: To call functions from Java: SendMessage(GameObject, Function, Parameters);
/// </summary>
public class WebHandler : Singleton<WebHandler> {
    // URL Split Character
    private static char[] URL_SPLIT = { '?' };

    // Invalid Parameeter Message. {Parameter}.
    private const string INVALID_PARAM_MSG = "Invalid Parameter: {0} is not a 'key=value' pair.";

    // URL Parameters.
    /// <summary>
    /// A dictionary of parameters found in the URL.
    /// If no parameters are set, the dictionary will be empty.
    /// </summary>
    public Dictionary<string, string> Param { get { return parameters; } }
    private Dictionary<string, string> parameters;

    /// <summary>
    /// URL Parameters in string format.
    /// If there are no parameters, get returns "";
    /// </summary>
    public string ParamString {
        get
        {
            if (!Application.absoluteURL.Contains("?")) return "";
            return Application.absoluteURL.Split(URL_SPLIT, 2)[1];
        }
    }


    /// <summary>
    /// Retrives any URL parameters on initiation.
    /// </summary>
    public void Init() {

        // Get Application URL
        string url = Application.absoluteURL;

        // Return if there are no parameters to add.
        if (url.IndexOf("?") == -1) return;

        // Get everything after the '?'.
        parameters = ToDictionary(url.Split(URL_SPLIT, 2)[1]);
    }

    public static Dictionary<string, string> ToDictionary(string paramString)
    {
        // Convert Parameters into dictionary.
        Dictionary<string, string> result = new Dictionary<string, string>();

        foreach (string param in paramString.Split('&'))
        {
            // 'Key=Value' pair.
            string[] pair = param.Split('=');

            // Check for proper array length:
            // * must have exactly 1 '='
            // * include both a key and value (neither 0-length).
            if (pair.Length == 2 && pair[0].Length > 0 && pair[1].Length > 0)
            {
                // Convert from escaped URL to standard string.
                result.Add(WWW.UnEscapeURL(pair[0]), WWW.UnEscapeURL(pair[1]));
            }
        }
        return result;
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
        foreach (KeyValuePair<string, string> pair in dictionary)
        {
            url += WWW.EscapeURL(pair.Key) + "&" + WWW.EscapeURL(pair.Value);
        }

        return url;
    }

    /// <summary>
    /// [Read-only Property]: If the platform running in WebGLPlayer.
    /// </summary>
    public bool IsWebMode { get { return Application.platform == RuntimePlatform.WebGLPlayer; } }

    /// <summary>
    /// [Read-only Property]: Retrives the canvas width.
    /// </summary>
    public int CanvasWidth { get { return JSCanvasWidth(); } }
    [DllImport("__Internal")]
    private static extern int JSCanvasWidth();

    /// <summary>
    /// [Read-only Property]: Retrives the canvas height.
    /// </summary>
    public int CanvasHeight { get { return JSCanvasHeight(); } }
    [DllImport("__Internal")]
    private static extern int JSCanvasHeight();

    /// <summary>
    /// Sends a JS message.alert(text) to the web browser.  Emulated in Unity Editor.
    /// </summary>
    /// <param name="text">Text MEssage</param>
    public void Alert(string text)
    {
        if (IsWebMode)
        {
            JSAlert(text);
        }
        else
        {
            // UnityEdtior alert emulation.
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog("JS Alert", text, "OK");
#endif
        }
    }
    [DllImport("__Internal")]
    private static extern void JSAlert(string text);

}
