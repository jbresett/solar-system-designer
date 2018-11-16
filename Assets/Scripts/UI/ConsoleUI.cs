using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles UI behvaior for the console.
/// </summary>
public class ConsoleUI : MonoBehaviour {
    
    // References to UI elements.
    private GameObject ConsolePanel;
    private InputField TextInput;
    private Text TextOutput;

    // Use this for initialization
    void Start () {

        // Set references to UI elements.
        ConsolePanel = GameObject.Find("ConsolePanel");
        TextInput = GameObject.Find("ConsoleInput").GetComponent<InputField>();
        TextOutput = GameObject.Find("ConsoleOutput").GetComponent<Text>();

        // Hide Console at start.
        ConsolePanel.SetActive(false);

        // Add "test" command processor and output. 
        Debugger.AddProcessor(processTest);
        Debugger.AddOutput(output);
    }
	
	// Update is called once per frame
	void Update () {

        // Check for ~ to toggle console.
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            ConsolePanel.SetActive(!ConsolePanel.activeSelf);
        }

        // Input text field.
        // CHeck if text field is visible and enter is pushed.
        if (Input.GetKeyUp(KeyCode.Return) && ConsolePanel.activeSelf)
        {
            Debugger.send(TextInput.text);
        }

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
