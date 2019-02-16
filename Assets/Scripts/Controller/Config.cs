using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : Singleton<Config> {

    public string Version
    {
        get { return version; }
        set { version = value; }
    }
    [SerializeField]
    protected string version;

    public GameObject BodyContainer { get { return bodyContainer; } }
    [SerializeField]
    protected GameObject bodyContainer;

    public GameObject BodyPrefab { get { return bodyPrefab; } }
    [SerializeField]
    protected GameObject bodyPrefab;

}
