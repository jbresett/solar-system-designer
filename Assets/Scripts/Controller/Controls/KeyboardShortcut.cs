using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This class is used to check for keyboard shortcuts and is used to enable 
/// and disable the debug console.
/// </summary>
public class KeyboardShortcut : MonoBehaviour {

	/// <summary>
	/// Function intializes the class and begins update function.
	/// </summary>
	void Start () {
		
	}
	
	/// <summary>
	/// Each frame the function checks for the ~ shortcut key to open the console
	/// window.
	/// </summary>
	void Update () {
		// Check for ~ to toggle console.
		if (Input.GetKeyUp(KeyCode.BackQuote))
		{
			//EditorUtility.DisplayDialog("Test", "test", "ok");
			foreach (GameObject obj in GameObject.Find("Canvas").GetComponentsInChildren<GameObject>(true))
			{
				if (obj.name == "ConsoleInput") obj.SetActive(true);
				//EditorUtility.DisplayDialog("Test", name, "ok");
			}
			//ConsolePanel.SetActive(!ConsolePanel.activeSelf);
		}
	}
}