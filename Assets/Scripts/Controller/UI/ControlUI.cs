using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlUI : MonoBehaviour {


	void Start () {
        Transform parent = transform.Find("Controls/Keyboard");
        foreach(Transform child in parent)
        {
            // Get related direction and KeyCode for each control.
            Direction dir = child.name.Enum<Direction>();
            KeyCode code = Sim.Settings.KeyControls[dir];
            
            // set placeholder values to default.
            Transform placeholder = child.FindDescendant("Placeholder");
            placeholder.GetComponent<TextMeshProUGUI>().SetText(code.ToString());

        }
    }

    void Update () {
        Transform parent = transform.Find("Controls/Keyboard");
        foreach (Transform child in parent)
        {
            // Get related direction and KeyCode for each control.
            Direction dir = child.name.Enum<Direction>();

            // Update if value exists
            Transform text = child.FindDescendant("TextMeshPro - InputField");
            TMP_InputField field = text.GetComponent<TMP_InputField>();
            string value = field.text;

            // Skip if no value is entered.
            if (value.Length > 0)
            {

                // Limit to single character.
                if (value.Length > 1)
                { 
                    value = value.Substring(value.Length - 1, 1);
                    field.text = value;
                }

                // Update Key Control
                try
                {
                    KeyCode code = value.ToUpper().Enum<KeyCode>();
                    Sim.Settings.KeyControls[dir] = code;

                }
                // Code Not recognized.
                catch (System.Exception)
                {
                    // clear text and log issue.
                    field.text = "";
                    Debugger.log("Unable to set " + dir + " to " + value + ". Returning UI to default value.");
                }

            }

        }

    }
}
