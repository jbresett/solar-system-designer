using System.Collections;
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
    
    
    // Camera Panning
    public KeyCode MoveUp
    {
        get { return moveUp;  }
        set { moveUp = value; }
    }
    [SerializeField]
    private KeyCode moveUp = KeyCode.W;
    
    public KeyCode MoveDown
    {
        get { return moveDown;  }
        set { moveDown = value; }
    }
    [SerializeField]
    private KeyCode moveDown = KeyCode.S;
    
    public KeyCode MoveLeft
    {
        get { return moveLeft;  }
        set { moveLeft = value; }
    }
    [SerializeField]
    private KeyCode moveLeft = KeyCode.A;
    
    public KeyCode MoveRight
    {
        get { return moveRight;  }
        set { moveRight = value; }
    }
    [SerializeField]
    private KeyCode moveRight = KeyCode.D;
    
    public KeyCode MoveIn
    {
        get { return moveIn;  }
        set { moveIn = value; }
    }
    [SerializeField]
    private KeyCode moveIn = KeyCode.F;
    
    public KeyCode MoveOut
    {
        get { return moveOut;  }
        set { moveOut = value; }
    }

    [SerializeField]
    private KeyCode moveOut = KeyCode.C;
    
    // Camera FOV
    public KeyCode ViewIn
    {
        get { return viewIn;  }
        set { viewIn = value; }
    }
    [SerializeField]
    private KeyCode viewIn = KeyCode.G;
    
    public KeyCode ViewOut
    {
        get { return viewOut;  }
        set { viewOut = value; }
    }

    [SerializeField]
    private KeyCode viewOut = KeyCode.V;
    // Camera Rotation
    public KeyCode RotUp
    {
        get { return rotUp;  }
        set { rotUp = value; }
    }
    [SerializeField]
    private KeyCode rotUp = KeyCode.UpArrow;
    
    public KeyCode RotDown
    {
        get { return rotDown;  }
        set { rotDown = value; }
    }
    [SerializeField]
    private KeyCode rotDown = KeyCode.DownArrow;
    
    public KeyCode RotLeft
    {
        get { return rotLeft;  }
        set { rotLeft = value; }
    }
    [SerializeField]
    private KeyCode rotLeft = KeyCode.LeftArrow;
    
    public KeyCode RotRight
    {
        get { return rotRight;  }
        set { rotRight = value; }
    }
    [SerializeField]
    private KeyCode rotRight = KeyCode.RightArrow;

    // Basic Contants
    const float KEYBOARD_MOVE = 1000F;
    private const float FOVAdjust = .1f;
    private const float rotateSpeed = 10f;
    
    private float dragSpeed = 10f;
    private Vector3 dragOriginRot;
    private Vector3 dragOriginPos;
    private float mouseZoomFactor = 1f;

    /// <summary>
    /// This function is used to update the frame once
    /// per second.
    /// </summary>
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

    /// <summary>
    /// This function Updates movement position based on the keyboard.
    /// </summary>
    void UpdateKeyboard()
    { 
        // Move Factor is based on Base movement speed * User Preferences.
        float MoveFactor = KEYBOARD_MOVE * Time.deltaTime * (float)Preferences.Keyboard.Movement;

        //Up
        if (Input.GetKey(moveUp))
        {
            Camera.main.transform.position += Camera.main.transform.up * MoveFactor;
        }

        //Down
        if (Input.GetKey(moveDown))
        {
            Camera.main.transform.position -= Camera.main.transform.up * MoveFactor;
        }

        //Left
        if (Input.GetKey(moveRight))
        {
            Camera.main.transform.position += Camera.main.transform.right * MoveFactor;
        }

        //Right
        if (Input.GetKey(moveLeft))
        {
            Camera.main.transform.position -= Camera.main.transform.right * MoveFactor;
        }

        // Move Foward
        if (Input.GetKey(moveIn))
        {
            Camera.main.transform.position += Camera.main.transform.forward * MoveFactor;
        }

        // Move Backward
        if (Input.GetKey(moveOut))
        {
            Camera.main.transform.position -= Camera.main.transform.forward * MoveFactor;
        }

        // Zoom In
        if (Input.GetKey(viewIn))
        {
            Camera.main.fieldOfView += FOVAdjust;
        }

        // Move Backward
        if (Input.GetKey(viewOut))
        {
            Camera.main.fieldOfView -= FOVAdjust;
        }

        // Rotate Camera (note, the vectors are correct despite not matching the key)
        if (Input.GetKey(rotLeft))
            Camera.main.transform.Rotate(Vector3.down,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(rotRight))
            Camera.main.transform.Rotate(Vector3.up,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(rotUp))
            Camera.main.transform.Rotate(Vector3.left,rotateSpeed*Time.deltaTime);
        if (Input.GetKey(rotDown))
            Camera.main.transform.Rotate(Vector3.right,rotateSpeed*Time.deltaTime);
    }

    /// <summary>
    /// This function Updates movement position based on the mouse.
    /// </summary>
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
