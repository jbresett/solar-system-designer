using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Config : MonoBehaviour {

    public string version = "1.0.0";
    public TextMeshProUGUI versionNumText;

    void Start(){
        VersionNum = GetComponent<TextMeshProUGUI>();
        setVersionNum(version);
    }

    public TextMeshProUGUI getVersionNum()
    {
        return versionNumText.text;
    }

    public void setVersionNum(string version)
    {
        versionNumText.text = version;
    }
}
