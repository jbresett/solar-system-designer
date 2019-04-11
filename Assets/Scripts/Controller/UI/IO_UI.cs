using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Import/Output User Interface
/// Handles importing and exporting to files.
/// </summary>
public class IO_UI : MonoBehaviour {

    public GameObject ImportPanel;
    private TMP_InputField ImportText;

    public GameObject ExportPanel;
    private TMP_InputField ExportText;

    /// <summary>
    /// Called from Import Menu Option
    /// </summary>
    public void ImportMenuClick()
    {
        ExportPanel.SetActive(false);
        ImportPanel.SetActive(true);
    }

    /// <summary>
    /// Called from Export Menu.
    /// </summary>
    public void ExportMenuClick()
    {
        ImportPanel.SetActive(false);
        ExportPanel.SetActive(true);

        // Set InputField to current state and activate.
        ExportText.text = Sim.Instance.State;
        ExportText.Select();
        ExportText.ActivateInputField();
    }

    /// <summary>
    /// Called from Export Ok Button.
    /// </summary>
    public void ImportOkBtnClick()
    {
        Sim.Instance.State = ImportText.text;
    }

    /// <summary>
    /// Called from Import Panel "Cancel" and Export Panel "Done".
    /// Closes panel.
    /// </summary>
    public void IO_CloseBtnClick()
    {
        ImportPanel.SetActive(false);
        ExportPanel.SetActive(false);
    }

    void Start () {
        // Set InputText Fields.
        ImportText = ImportPanel.GetComponentInChildren<TMP_InputField>(true);
        ExportText = ExportPanel.GetComponentInChildren<TMP_InputField>(true);
    }

}
