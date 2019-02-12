using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to alter the display settings of the UI
/// </summary> 
public class SettingsUI : MonoBehaviour
{

	public float opacity = 0.7f;
	private List<Image> backgrounds = new List<Image>();
	public Slider opacitySlider;

	/// <summary>
	/// This function initializes the class
	/// </summary>
	private void Start ()
	{
		var images = Resources.FindObjectsOfTypeAll<Image>();
		foreach (var image in images)
		{
			if (image.tag == "UIMenu")
			{
				backgrounds.Add(image);
			}
		}

        foreach (var background in backgrounds)
        {
            var color = background.color;
            color.a = opacity;
            background.color = color;
        }

        opacitySlider.onValueChanged.AddListener(delegate {updateOpacity();});
		updateOpacity();
	}

	/// <summary>
	/// Function updates the opacity and background color of the UI
	/// </summary>
	private void updateOpacity()
	{
		opacity = opacitySlider.value;
		foreach (var background in backgrounds)
		{
            var color = background.color;
			color.a = opacity;
			background.color = color;
        }
	}
}