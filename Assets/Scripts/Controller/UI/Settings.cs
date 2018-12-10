using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

	public float opacity = .9f;
	private List<Image> backgrounds = new List<Image>();
	public Slider opacitySlider;
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
		opacitySlider.onValueChanged.AddListener(delegate {updateOpacity();});
		updateOpacity();
	}

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