using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Config : Singleton<Config> {

    public string version = "1.0.0";
    public TextMeshProUGUI versionNumText;

    void Start(){
        VersionNum = GetComponent<TextMeshProUGUI>();
        setVersionNum(version);
    }

    public TextMeshProUGUI getVersionNum()
    {
        get { return FindObjectOfType<Sim>().version; }
        set { FindObjectOfType<Sim>().version = value; }
    }

}
