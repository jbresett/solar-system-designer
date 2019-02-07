using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour {

    string versionNum = "1.0.0";

    public string updateVersion()
    {
        get { return versionNum; }
        //set {versionNum = placeholder;}
    }
    private string versionNum = "1.1.0";
}
