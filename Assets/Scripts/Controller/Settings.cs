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
public enum SpeedRatio
{
    Second = 1,
    Minute = 60,
    Hour = 3600,
    Day = 86400,
    Month = 2629800,
    Year = 31557600,
} 

public class Settings : Singleton<Settings>
{
    public GameObject speedBar; 

    // Links a Move Direction to a key control
    public Dictionary<Direction, KeyCode> KeyControls
    {
        get { return controlMap; }
    }

    public bool Paused
    {
        get { return paused; }
        set {
            paused = value;
            var ui = speedBar.GetComponent<SpeedUI>();
            ui.pauseButton.interactable = !Sim.Settings.Paused;
            ui.playButton.interactable = Sim.Settings.Paused;
            if (!Application.isPlaying) return; // No Capi interface during edit mode.
            Sim.Capi.Exposed.capiPaused.setValue(value);
        }
    }
    [SerializeField]
    protected bool paused;
    
    public double Speed
    {
        get { return speed; }
        set
        {
            speed = value;
            speedBar.GetComponent<SpeedUI>().UpdateSpeedText();

            if (!Application.isPlaying) return; // No Capi interface during edit mode.

            // Use Highest Ratio possible when setting values.
            SpeedRatio useRatio = SpeedRatio.Second;
            foreach (SpeedRatio ratio in System.Enum.GetValues(typeof(SpeedRatio)))
            {
                if (value >= (int)ratio && ratio > useRatio)
                {
                    useRatio = ratio;
                }
            }
            Sim.Capi.Exposed.capiSpeedTime.setValue((float)(value / (float)useRatio));
            Sim.Capi.Exposed.capiSpeedRatio.setValue(useRatio);
        }
    }
    [SerializeField]
    protected double speed = (float)SpeedRatio.Day;

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

    public void Awake()
    {
        speedBar.GetComponent<SpeedUI>().OnSliderChange((float)Speed);
    }

}
