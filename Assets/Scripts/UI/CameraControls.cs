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
    private const float FOVAdjust = .1f;
    private const float rotateSpeed = 1f;

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
            Camera.main.fieldOfView += FOVAdjust;
        }

        // Move Backward
        if (Input.GetKey(KeyCode.V))
        {
            Camera.main.fieldOfView -= FOVAdjust;
        }
        // Rotate Camera (note, the vectors are correct despite not matching the key)
        if (Input.GetKey(KeyCode.LeftArrow))
            Camera.main.transform.Rotate(Vector3.down,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            Camera.main.transform.Rotate(Vector3.up,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(KeyCode.UpArrow))
            Camera.main.transform.Rotate(Vector3.left,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(KeyCode.DownArrow))
            Camera.main.transform.Rotate(Vector3.right,rotateSpeed*Time.deltaTime);
    }
}
