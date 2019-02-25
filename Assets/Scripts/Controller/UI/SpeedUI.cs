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
    public Slider slider;
    public Button pauseButton, playButton;
    public TextMeshProUGUI speedText;

	// Use this for initialization
	void Start () {

        speedText.text = "0";
        slider.GetComponent<Slider>().value = Sim.Settings.Speed;
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
        // Swaps which button is enabled.
        pauseButton.interactable = !Sim.Settings.Paused;
        playButton.interactable = Sim.Settings.Paused;
    }

    /// <summary>
    /// Updates the speed text. Automatically called when Slider is changed.
    /// </summary>
    public void UpdateSpeedText()
    {
        // Current Speed.
        float speed = (int)Sim.Settings.Speed;
    
        // Ratio to use based on current speed context.
        SpeedRatios useRatio = SpeedRatios.Stop;

        // If sim is stopped, return 0 (no ratio).
        if (speed == 0)
        {
            speedText.text = "0";
            return;
        }


        // Find Current Ratio: Is  in hours, days, etc.
        foreach (SpeedRatios ratio in (SpeedRatios[])Enum.GetValues(typeof(SpeedRatios)))
        {
            if (speed >= (int)ratio && (int)ratio > (int)useRatio)
            {
                useRatio = ratio;
            }
        }

        // Set display speed based on ratio (e.g. Hours/sec).
        speed = speed / (float)useRatio;

        // Update display
        speedText.text = string.Format("{0:0.0} {1}/sec", speed, useRatio.ToString());

    }

    /// <summary>
    /// Resets all Planets to initial positions.
    /// </summary>
    public void OnResetButtonPress()
    {
        foreach (Body body in Sim.Bodies.getAll())
        {
            body.Position = body.InitialPosition;
        }
    }

}
