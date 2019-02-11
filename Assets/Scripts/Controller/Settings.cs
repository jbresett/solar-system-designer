using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control Directions.
public enum Direction
{
    MoveUp, MoveDown, MoveLeft, MoveRight, MoveForward, MoveBackward,
    ZoomIn, ZoomOut,
    RotLeft, RotRight, RotUp, RotDown
}

public class Settings : Singleton<Settings> {

    // Links a Move Direction to a key control
    public Dictionary<Direction, KeyCode> KeyControls
    {
        get { return controlMap; }
    }
    // Initialize with Default Values.
    private Dictionary<Direction, KeyCode> controlMap = new Dictionary<Direction, KeyCode>()
    {
        {Direction.MoveUp, KeyCode.W },
        {Direction.MoveDown, KeyCode.S },
        {Direction.MoveLeft, KeyCode.A },
        {Direction.MoveRight, KeyCode.D },
        {Direction.MoveForward, KeyCode.F },
        {Direction.MoveBackward, KeyCode.C },
        {Direction.ZoomIn, KeyCode.G },
        {Direction.ZoomOut, KeyCode.V },
        {Direction.RotLeft, KeyCode.LeftArrow },
        {Direction.RotRight, KeyCode.RightArrow },
        {Direction.RotUp, KeyCode.UpArrow },
        {Direction.RotDown, KeyCode.DownArrow }
    };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
