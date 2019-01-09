using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to set different preferences in
/// the simulation.
/// </summary>
public class PreferencesUI : MonoBehaviour {

	/// <summary>
    /// Use this for initialization
    /// </summary>
	void Start () {
		// Nothing needed here at this time.
        // TODO: When preferences are saved between runs, we'll need to load the values here.
	}
	
	/// <summary>
    /// Update is called once per frame
    /// </summary>
	void Update () {

        // While the Preferences UI window is visible, updates all preferences.

        updateSpeeds(Preferences.Keyboard, "Keyboard");
        updateSpeeds(Preferences.Mouse, "Mouse");
    }

    /// <summary>
    /// this function adjusts speed of slider rotation, movement and zoom
    /// </summary>
    private void updateSpeeds(Preferences.Speed speed, string name)
    {
        speed.Movement = GameObject.Find(name + "Movement/Slider").GetComponent<Slider>().value;
        speed.Rotation = GameObject.Find(name + "Rotation/Slider").GetComponent<Slider>().value;
        speed.Zoom = GameObject.Find(name + "Zoom/Slider").GetComponent<Slider>().value;
    }
}
