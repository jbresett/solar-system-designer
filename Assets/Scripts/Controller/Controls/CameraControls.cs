﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class handles all movement and control of the camera
/// </summary>
public class CameraControls : MonoBehaviour {

    /// <summary>
    /// Method used to enable and disable the keyboard
    /// </summary>
    public bool EnableKeyboard
    {
        get { return enableKeyboard; }
        set { enableKeyboard = value; }
    }
    [SerializeField]
    private bool enableKeyboard = true;

    /// <summary>
    /// This method is used to enable and disable the
    /// mouse functionality
    /// </summary>
    public bool EnableMouse
    {
        get { return enableMouse;  }
        set { enableMouse = value; }
    }
    [SerializeField]
    private bool enableMouse = true;
    
    // Basic Constants
    const float KEYBOARD_MOVE = 1000F;
    private const float FOVAdjust = .1f;
    private const float rotateSpeed = 10f;
    
    private float dragSpeed = 10f;
    private Vector3 dragOriginRot;
    private Vector3 dragOriginPos;
    private float mouseZoomFactor = 1f;
    
    private List<Image> backgrounds = new List<Image>();

    private void Start()
    {
        var images = Resources.FindObjectsOfTypeAll<Image>();
        foreach (var image in images)
        {
            if (image.tag == "UIMenu")
            {
                backgrounds.Add(image);
            }
        }
    }

    /// <summary>
    /// This function is used to update the frame once
    /// per second.
    /// </summary>
    void Update()
    {
        // Automatically disable Cemera controls if the current object is an InputField.
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        enableKeyboard = true;
        enableMouse = true;
        foreach(Image i in backgrounds)
        {
            if (i.isActiveAndEnabled)
            {
                enableKeyboard = false;
                enableMouse = false;
                break;
            }
        }
        
        if (EnableKeyboard && (selected == null || !selected.GetComponent<InputField>()))
        {
            UpdateKeyboard();
        }

        if (EnableMouse)
            UpdateMouse();
    }

    /// <summary>
    /// This function Updates movement position based on the keyboard.
    /// </summary>
    void UpdateKeyboard()
    { 
        // Move Factor is based on Base movement speed * User Preferences.
        float MoveFactor = KEYBOARD_MOVE * Time.deltaTime * (float)Preferences.Keyboard.Movement;

        // Get Controls
        var controls = Sim.Settings.KeyControls;
        
        //Up
        if (Input.GetKey(controls[Direction.MoveUp]))
        {
            Camera.main.transform.position -= Camera.main.transform.up * MoveFactor;
        }

        //Down
        if (Input.GetKey(controls[Direction.MoveDown]))
        {
            Camera.main.transform.position += Camera.main.transform.up * MoveFactor;
        }

        //Left
        if (Input.GetKey(controls[Direction.MoveLeft]))
        {
            Camera.main.transform.position -= Camera.main.transform.right * MoveFactor;
        }

        //Right
        if (Input.GetKey(controls[Direction.MoveRight]))
        {
            Camera.main.transform.position += Camera.main.transform.right * MoveFactor;
        }

        // Move Forward
        if (Input.GetKey(controls[Direction.MoveForward]))
        {
            Camera.main.transform.position += Camera.main.transform.forward * MoveFactor;
        }

        // Move Backward
        if (Input.GetKey(controls[Direction.MoveBackward]))
        {
            Camera.main.transform.position -= Camera.main.transform.forward * MoveFactor;
        }

        // Zoom In
        if (Input.GetKey(controls[Direction.ZoomIn]))
        {
            Camera.main.fieldOfView += FOVAdjust;
        }

        // Move Backward
        if (Input.GetKey(controls[Direction.ZoomOut]))
        {
            Camera.main.fieldOfView -= FOVAdjust;
        }

        // Rotate Camera (note, the vectors are correct despite not matching the key)
        if (Input.GetKey(controls[Direction.RotLeft]))
            Camera.main.transform.Rotate(Vector3.down,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(controls[Direction.RotRight]))
            Camera.main.transform.Rotate(Vector3.up,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(controls[Direction.RotUp]))
            Camera.main.transform.Rotate(Vector3.left,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(controls[Direction.RotDown]))
            Camera.main.transform.Rotate(Vector3.right,rotateSpeed*Time.deltaTime);
    }

    /// <summary>
    /// This function Updates movement position based on the mouse.
    /// </summary>
    private void UpdateMouse()
    {
        Camera.main.transform.position += Camera.main.transform.forward*Input.mouseScrollDelta.y*mouseZoomFactor;
        
        if (Input.GetMouseButton(0) && !dragOriginPos.Equals(Vector3.negativeInfinity))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOriginPos);
            pos.y = -pos.y;
            pos.x = -pos.x;
            Camera.main.transform.Translate(pos*dragSpeed);
            Debug.Log(pos);
            return;
        }
        else
        {
            dragOriginPos = Vector3.negativeInfinity;
        }
        if (Input.GetMouseButton(1) && !dragOriginRot.Equals(Vector3.negativeInfinity))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOriginRot);
            float x = pos.x;
            float y = pos.y;
            pos.x = y;
            pos.y = -x;
            Debug.Log(pos);
            Camera.main.transform.Rotate(pos);
            return;
        }
        else
        {
            dragOriginRot = Vector3.negativeInfinity;
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragOriginPos = Input.mousePosition;
            Debug.Log(dragOriginPos);
        }
        if (Input.GetMouseButtonDown(1))
        {
            dragOriginRot = Input.mousePosition;
        }
        
    }
}
