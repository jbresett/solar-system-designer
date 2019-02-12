using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Config : Singleton<Config> {
    
    void Start(){
    }

    public string Version
    {
        get { return FindObjectOfType<Sim>().version; }
        set { FindObjectOfType<Sim>().version = value; }
    }

}
