using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyboardShortcut : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Check for ~ to toggle console.
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            EditorUtility.DisplayDialog("Test", "test", "ok");
            foreach (GameObject obj in GameObject.Find("Canvas").GetComponentsInChildren<GameObject>(true))
            {
                if (obj.name == "ConsoleInput") obj.SetActive(true);
                EditorUtility.DisplayDialog("Test", name, "ok");
            }
            //ConsolePanel.SetActive(!ConsolePanel.activeSelf);
        }
    }
}
