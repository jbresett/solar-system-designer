using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimCapiMono : MonoBehaviour
{
    public SimCapi.Transporter transporter;

    public void receiveMessage(string message)
    {
        transporter.getMessagePipe().receiveMessage(message);
    }

    public void Update()
    {
        if(transporter != null)
            transporter.update(Time.unscaledDeltaTime);
    }
}
