using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control Directions.
/// </summary>
public enum Direction
{
    MoveUp, MoveDown, MoveLeft, MoveRight, MoveForward, MoveBackward,
    ZoomIn, ZoomOut,
    RotLeft, RotRight, RotUp, RotDown
}

/// <summary>
/// Time Ratios for Speed.  Used by the Capi interface default options (can be manually set in Capi as well).
/// </summary>
public enum SpeedRatios
{
    Stop = 0,
    Second = 1,
    Minute = 60,
    Hour = 3600,
    Day = 86400,
    Month = 2629800,
    Year = 31557600,
} 

public class Settings : Singleton<Settings>
{

    // Links a Move Direction to a key control
    public Dictionary<Direction, KeyCode> KeyControls
    {
        get { return controlMap; }
    }

    public bool Paused
    {
        get { return paused; }
        set { paused = value; }
    }
    [SerializeField]
    protected bool paused;
    
    public float Speed
    {
        get { return speed; }
        set
        {
            Sim.Capi.Exposed.Speed.setValue(value);
            speed = value;
        }
    }
    [SerializeField]
    protected float speed = (float)SpeedRatios.Day;

    // Initialize with Default Values.
    [SerializeField]
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

}
