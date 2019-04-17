using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class links Speed Bar Features (buttons/sliders) to Sim.Settings
/// </summary>
public class SpeedUI : MonoBehaviour {
    private const string SPEED_DISPLAY_FMT = "{0:0.0} {1}{2}/sec"; // Value, Ratio, Plural

    public Slider slider;
    public Button pauseButton, playButton;
    public TextMeshProUGUI speedText;

	// Use this for initialization
	void Start () {

        speedText.text = "0";
        slider.GetComponent<Slider>().value = (float)Sim.Settings.Speed;
        pauseButton.interactable = !Sim.Settings.Paused;
        playButton.interactable = Sim.Settings.Paused;
    }

    /// <summary>
    /// Triggers on the speed slider change.
    /// </summary>
    /// <param name="value"></param>
    public void OnSliderChange(float value)
    {
        Sim.Settings.Speed = value;
        UpdateSpeedText();
    }

    /// <summary>
    /// Triggers on Pause or Unpause button press.
    /// </summary>
    /// <param name="Paused"></param>
    public void OnButtonPress(bool Paused)
    {
        Sim.Settings.Paused = Paused;
    }

    /// <summary>
    /// Updates the speed text. Automatically called when Slider is changed.
    /// </summary>
    public void UpdateSpeedText()
    {
        // Current Speed.
        float speed = (int)Sim.Settings.Speed;
    
        // Ratio to use based on current speed context.
        SpeedRatio useRatio = SpeedRatio.Second;

        // Find Current Ratio: Is  in hours, days, etc.
        foreach (SpeedRatio ratio in (SpeedRatio[])Enum.GetValues(typeof(SpeedRatio)))
        {
            if (speed >= (int)ratio && (int)ratio > (int)useRatio)
            {
                useRatio = ratio;
            }
        }

        // Set display speed based on ratio (e.g. Hours/sec).
        speed = speed / (float)useRatio;

        // Update display: Value, Ratio, Plural (s)
        speedText.text = string.Format(SPEED_DISPLAY_FMT, speed, useRatio.ToString(), (speed == 1.0 ? "" : "s"));

    }

    /// <summary>
    /// Resets all Planets to initial positions.
    /// </summary>
    public void OnResetButtonPress()
    {
        Sim.Instance.Reset();
    }

}
