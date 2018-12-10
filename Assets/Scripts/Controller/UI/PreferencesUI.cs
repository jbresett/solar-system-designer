using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ha
/// </summary>
public class PreferencesUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Nothing needed here at this time.
        // TODO: When preferences are saved between runs, we'll need to load the values here.
	}
	
	// Update is called once per frame
	void Update () {

        // While the Preferences UI window is visible, updates all preferences.

        updateSpeeds(Preferences.Keyboard, "Keyboard");
        updateSpeeds(Preferences.Mouse, "Mouse");
    }

    private void updateSpeeds(Preferences.Speed speed, string name)
    {
        speed.Movement = GameObject.Find(name + "Movement/Slider").GetComponent<Slider>().value;
        speed.Rotation = GameObject.Find(name + "Rotation/Slider").GetComponent<Slider>().value;
        speed.Zoom = GameObject.Find(name + "Zoom/Slider").GetComponent<Slider>().value;
    }
}
