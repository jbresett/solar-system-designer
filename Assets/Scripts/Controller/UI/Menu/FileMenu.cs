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

    /// <summary>
    /// Called from Import Menu Option
    /// Displays either prompt (WebGL) or window (other modes).
    /// </summary>
    public void ImportMenuClick()
    {
        // if in WebGL, use JS Prompt instead.
        if (Sim.Web.IsWebMode)
        {
            string result = Sim.Web.Prompt("Paste from clipboard.", "");
            if (!string.IsNullOrEmpty(result))
            {
                State.Instance.Current = result;
            }
        }
        else
        {
            ExportPanel.SetActive(false);
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
        // if in WebGL, use JS Prompt instead.
        if (Sim.Web.IsWebMode)
        {
            bool useURL = Sim.Web.Confirm("Include URL ('OK' for Yes, 'Cancel' for no)?");
            string message = (useURL ? Sim.Web.URL + "?" : "") + State.Instance.Current;
            Sim.Web.Prompt("Copy to Clipboard:", message);
        }
        else
        {
            ImportPanel.SetActive(false);
            ExportPanel.SetActive(true);
            // Set InputField to current state and focus.
            ExportText.text = State.Instance.Current;
            ExportText.Select();
            ExportText.ActivateInputField();
        }
    }

    /// <summary>
    /// Called from Export Ok Button.
    /// </summary>
    public void ImportOkBtnClick()
    {
        State.Instance.Current = ImportText.text;
        ImportPanel.SetActive(false);
    }

    void Start () {
        // Set InputText Fields.
        ImportText = ImportPanel.GetComponentInChildren<TMP_InputField>(true);
        ExportText = ExportPanel.GetComponentInChildren<TMP_InputField>(true);
    }

}
