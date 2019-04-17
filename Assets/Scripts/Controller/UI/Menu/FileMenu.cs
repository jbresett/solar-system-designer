using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Import/Output User Interface
/// Handles importing and exporting to files.
/// </summary>
public class FileMenu : MonoBehaviour {

    public GameObject ImportPanel;
    private TMP_InputField ImportText;

    public GameObject ExportPanel;
    private TMP_InputField ExportText;
    private Toggle ExportUrlToggle;

    /// <summary>
    /// Called from Import Menu Option
    /// Displays either prompt (WebGL) or window (other modes).
    /// </summary>
    public void ImportMenuClick()
    {
        if (Sim.Web.IsWebMode)
        {
            string result = Sim.Web.Prompt("Paste from clipboard.", "");
            if (result != null)
            {
                Sim.Instance.State = result;
            }
        }
        else
        {
            ImportPanel.SetActive(true);

            // Set InputField to current state and focus.
            ImportText.Select();
            ImportText.ActivateInputField();
        }
  
    }

    /// <summary>
    /// Called from Export Menu.
    /// Displays either prompt (WebGL) or window (other modes).
    /// </summary>
    public void ExportMenuClick()
    {
        // if in WebGL, use JS actPrompt instead.
        if (Sim.Web.IsWebMode)
        {
            bool useURL = Sim.Web.Confirm("Include URL?");
            string message = (useURL ? Sim.Web.URL : "") + Sim.Instance.State;
            Sim.Web.Prompt("Copy to Clipboard:", message);
        }
        else
        {
            ExportPanel.SetActive(true);

            // Set InputField to current state and focus.
            ExportText.text = Sim.Instance.State;
            ExportText.Select();
            ExportText.ActivateInputField();
        }
    }

    /// <summary>
    /// Called from Export Ok Button.
    /// </summary>
    public void ImportOkBtnClick()
    {
        Sim.Instance.State = ImportText.text;
    }

    void Start () {
        // Set InputText Fields.
        ImportText = ImportPanel.GetComponentInChildren<TMP_InputField>(true);
        ExportText = ExportPanel.GetComponentInChildren<TMP_InputField>(true);
        ExportUrlToggle = ExportPanel.GetComponentInChildren<Toggle>();
    }

}
