using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all Camera Controls.
/// </summary>
public class CameraControls : MonoBehaviour {
    public bool EnableKeyboard = true;


    // Basic Contants
    const float KEYBOARD_MOVE = 1000F;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        if (EnableKeyboard)
        {
            UpdateKeyboard();
        }
    }

    // Updates movement position based on the keyboard.
    void UpdateKeyboard()
    { 
        // Move Factor is based on Base movement speed * User Preferences.
        float MoveFactor = KEYBOARD_MOVE * Time.deltaTime * (float)Preferences.Keyboard.Movement;

        //Up
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position += Camera.main.transform.up * MoveFactor;
        }

        //Down
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position -= Camera.main.transform.up * MoveFactor;
        }

        //Left
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position += Camera.main.transform.right * MoveFactor;
        }

        //Right
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position -= Camera.main.transform.right * MoveFactor;
        }

        // Move Foward
        if (Input.GetKey(KeyCode.F))
        {
            Camera.main.transform.position += Camera.main.transform.forward * MoveFactor;
        }

        // Move Backward
        if (Input.GetKey(KeyCode.C))
        {
            Camera.main.transform.position -= Camera.main.transform.forward * MoveFactor;
        }

        // Zoom In
        if (Input.GetKey(KeyCode.G))
        {
            Camera.main.fieldOfView += 1;
        }

        // Move Backward
        if (Input.GetKey(KeyCode.V))
        {
            Camera.main.fieldOfView -= 1;
        }
    }
}
