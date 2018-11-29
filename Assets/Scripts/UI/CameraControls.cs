using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles all Camera Controls.
/// </summary>
public class CameraControls : MonoBehaviour {
    public bool EnableKeyboard = true;
    public bool EnableMouse = true;

    // Basic Contants
    const float KEYBOARD_MOVE = 1000F;
    private const float FOVAdjust = .1f;
    private const float rotateSpeed = 10f;
    
    private float dragSpeed = 10f;
    private Vector3 dragOriginRot;
    private Vector3 dragOriginPos;
    private float mouseZoomFactor = 1f;

    // Update is called once per frame
    void Update()
    {
        // Automatically disable Cemera controls if the current object is an InputField.
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        
        if (EnableKeyboard && (selected == null || !selected.GetComponent<InputField>()))
        {
            UpdateKeyboard();
        }

        if (EnableMouse)
            UpdateMouse();
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

    private void UpdateMouse()
    {
        Camera.main.transform.position += Camera.main.transform.forward*Input.mouseScrollDelta.y*mouseZoomFactor;
        
        if (Input.GetMouseButtonDown(0))
        {
            dragOriginPos = Input.mousePosition;
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            dragOriginRot = Input.mousePosition;
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOriginPos);
            transform.Translate(pos*dragSpeed);
            return;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOriginRot);
            float x = pos.x;
            float y = pos.y;
            pos.x = y;
            pos.y = -x;
            transform.Rotate(pos);
        }
    }
}
