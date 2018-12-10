using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    public float rotSpd;
    public float dampAmt;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate((Vector3.up * rotSpd) * (Time.deltaTime * dampAmt), Space.Self);
        //transform.Rotate((Vector3.up * rotSpd) * (Time.deltaTime * dampAmt), Space.World);
        transform.RotateAround(Vector3.zero, transform.up, Time.deltaTime * dampAmt);
    }
}

