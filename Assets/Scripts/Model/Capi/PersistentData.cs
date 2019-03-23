using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimCapi;

/// <summary>
/// Basic handler for processing SimCapi persistent data.
/// </summary>
public class PersistentData: Singleton<PersistentData>
{

    /// <summary>
    /// Sets a persisent value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    public void Set(string key, string value, SetDataRequestSuccessDelegate onSuccess, SetDataRequestErrorDelegate onError)
    {

        Sim.Capi.Transporter.setDataRequest(Capi.SIM_ID, key, value, onSuccess, onError);
    }

    /// <summary>
    /// Returns a persisent value.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="onSuccess"></param>
    /// <param name="onError"></param>
    public void Get(string key, GetDataRequestSuccessDelegate onSuccess, GetDataRequestErrorDelegate onError)
    {
        Sim.Capi.Transporter.getDataRequest(Capi.SIM_ID, key, onSuccess, onError);
    }

}
