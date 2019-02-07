using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour {

    public string Version
    {
        get { return versionNum; }
        //set {versionNum = placeholder;}
    }
    private string versionNum = "1.1.0";
}
