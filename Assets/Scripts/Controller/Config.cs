using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : Singleton<Config> {

    public string Version
    {
        get { return FindObjectOfType<Sim>().version; }
        set { FindObjectOfType<Sim>().version = value; }
    }

}
