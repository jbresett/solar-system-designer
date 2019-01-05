using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    public float RotSpd
    {
        get { return rotSpd; }
        set { rotSpd = value; }
    }
    [SerializeField]
    private float rotSpd;

    public float DampAmt
    {
        get { return dampAmt; }
        set { dampAmt = value; }
    }
    [SerializeField]
    private float dampAmt;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate((Vector3.up * RotSpd) * (Time.deltaTime * DampAmt), Space.Self);
        //transform.Rotate((Vector3.up * rotSpd) * (Time.deltaTime * dampAmt), Space.World);
        transform.RotateAround(Vector3.zero, transform.up, Time.deltaTime * DampAmt);
    }
}

