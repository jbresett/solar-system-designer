using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;

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
    /// If no parameters are set, Param will return a null.
    /// </summary>
    public Dictionary<string, string> Param { get { return parameters; } }
    private Dictionary<string, string> parameters;

    public string URL
    {
        get { return Application.absoluteURL.Split(URL_SPLIT, 2)[0]; }
    }

    /// <summary>
    /// URL Parameters in string format.
    /// If there are no parameters, get returns "";
    /// </summary>
    public string URLStateString {
        get
        {
            if (!Application.absoluteURL.Contains("?")) return "";
            return Application.absoluteURL.Split(URL_SPLIT, 2)[1];
        }
    }

    /// <summary>
    /// Generates a URL based on the current URL and a system state string.
    /// </summary>
    /// <param name="dictionary">dictionary to use. If null or not set, the current dictionary will be used.</param>
    /// <returns></returns>
    public string ToUrl(string state)
    {
        return Application.absoluteURL.Split('?')[0] + "?" + state;
    }

    /// <summary>
    /// Retrives any URL parameters on initiation.
    /// </summary>
    public void Awake() {
        // Get Application URL
        string url = Application.absoluteURL;

        // Add parameter list
        if (url.IndexOf("?") >= 0)
        { 
            parameters = State.ToDictionary(url.Split(URL_SPLIT, 2)[1]);
        }
    }

    public void Start()
    {
        // Check for Parameters in URL. If found, use for initial state.
        if (Param != null)
        {
            // Update Simulation with URL values.
            State.Instance.Current = URLStateString;
        }
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
    /// <param name="message">Text MEssage</param>
    public void Alert(string message)
    {
        if (IsWebMode)
        {
            JSAlert(message);
        }
        else
        {
            // UnityEdtior alert emulation.
#if UNITY_EDITOR
            UnityEditor.EditorUtility.DisplayDialog("JS Alert", message, "OK");
#endif
        }
    }
    [DllImport("__Internal")]
    private static extern void JSAlert(string text);


    /// <summary>
    /// Displays a JS Confirm for the web browser.
    /// No Unity Editor emulation (returns false).
    /// </summary>
    public bool Confirm(string message)
    {
        if (IsWebMode)
        {
            return JSConfirm(message);
        }
        else
        {
            return false;
        }
    }
    [DllImport("__Internal")]
    private static extern bool JSConfirm(string message);

    /// <summary>
    /// Displays a JS prompt for the web browser.
    /// No Unity Editor emulation (returns defaultText).
    /// </summary>
    /// <param name="message"></param>
    /// <param name="defaultText">Default value</param>
    /// <returns></returns>
    public string Prompt(string message, string defaultText)
    {
        if (IsWebMode)
        {
            return JSPrompt(message, defaultText);
        }
        else
        {
            return defaultText;
        }
    }
    [DllImport("__Internal")]
    private static extern string JSPrompt(string message, string defaultText);

    /// <summary>
    /// Sets a Property (Lambda) to a dictionary value while converting the value to the choosen type.
    /// </summary>
    /// <typeparam name="T">Type to convert to</typeparam>
    /// <param name="setter">Lambda setter. Use: <code>v => Property = v</code></param>
    /// <param name="dictionary">Dictionary</param>
    /// <param name="key">Key value</param>
    /// <returns>True if the property was set, false if the key does not exist.</returns>
    private bool SetProperty<T>(System.Action<T> setter, Dictionary<string, string> dictionary, string key)
    {
        if (!dictionary.ContainsKey(key)) return false;
        T value = (T)System.Convert.ChangeType(dictionary[key], typeof(T));
        setter(value);
        return true;
    }
}
