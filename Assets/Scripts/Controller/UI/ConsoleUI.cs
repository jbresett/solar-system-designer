using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI behvaior for the console.
/// </summary>
public class ConsoleUI : MonoBehaviour {
    
    // Set in Unity
    public GameObject ConsolePanel;
    public InputField TextInput;
    public Text TextOutput;

    /// <summary>
    /// This function initializes the class
    /// </summary>
    void Start () {

        // Add "test" command processor and output. 
        Debugger.AddProcessor(processTest);
        Debugger.AddOutput(output);
    }
	
	/// <summary> 
    /// Updates console once per frame
    /// </summary>
	void Update () {

        // Input text field.
        // CHeck if text field is visible and enter is pushed.
        if (Input.GetKeyUp(KeyCode.Return) && ConsolePanel.activeSelf)
        {
            Debugger.send(TextInput.text);
        }

        gameObject.GetComponent<CameraControls>().EnableKeyboard = !TextInput.isFocused;
    }

    /// <summary>
    /// Processes any command output to the UI console.
    /// 
    /// Called by the Debugger class.
    /// </summary>
    /// <param name="lines"></param>
    public void output(string[] lines)
    {
        string result = "";
        foreach (string line in lines)
        {
            result += line + "\n";
        }
        TextOutput.text = result;
    }
    
    /// <summary>
    /// Sends any Debugger "test" command back with all the arguments.
    /// 
    /// Called by the Debugger.
    /// </summary>
    /// <param name="arguments"></param>
    /// <returns></returns>
    public string[] processTest(string[] arguments)
    {
        if (arguments[0] == "test")
        {
            return arguments;
        }
        else
        {
            return new string[0];
        }
    }
}
