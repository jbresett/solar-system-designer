using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

public class CapiController : MonoBehaviour
{
    public SimCapi.Transporter MyTransporter;
    public SimCAPIExposeData MyCAPIData;

    public bool FirstUpdate = false;

    private int visits = 0;

	// Use this for initialization
	void Start ()
    {
        MyTransporter = SimCapi.Transporter.getInstance();

        MyTransporter.notifyOnReady();

        MyCAPIData = new SimCAPIExposeData();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(!FirstUpdate)
        {
            FirstUpdate = true;
            InitializeCAPI();            
        }
	}

    public void InitializeCAPI()
    {
        Debug.Log("Starting CAPI Initialization:" + visits);

        #region Exposes
        // Example expose
        //MyCAPIData.StarsBorn.expose("R-starsBorn.value", false, false);

        #endregion

        #region Delegates

        // Example Delegate
        /*
        MyCAPIData.StarsBorn.setChangeDelegate(delegate (float value, SimCapi.ChangedBy changedBy)
        {
            value = Mathf.Clamp(value, MyDrakeEquation.sliders[0].minValue, MyDrakeEquation.sliders[0].maxValue);

            if (changedBy == SimCapi.ChangedBy.AELP)
            {
                Debug.Log("Setting value for StarsBorn :" + changedBy.ToString() + " changed state to " + value.ToString());
                MyDrakeEquation.sliders[0].value = value;
                //Globals.starsInMilkyWay = value;
            }
        }
        );
        */

        #endregion

        #region Value Initialization

        // Not strictly necessary unless you must initialize from outside the SimCAPIExposeData class
        //UpdateCAPISceneControllerValues();

        #endregion

        Debug.Log("CAPI Initialization Complete.");
    }

    public void UpdateCAPISceneControllerValues()
    {
        // Put generic update calls for events here
    }

    public class SetRequestDelegates
    {
        public void onSuccess(SimCapi.Message.SetDataResponse setDataResponse)
        {
            // Value was set successfully
        }

        public void onError(SimCapi.Message.SetDataResponse setDataResponse)
        {
            // Error occured value not set
        }
    }


    public class GetRequestDelegates
    {
        public void onSuccess(SimCapi.Message.GetDataResponse setDataResponse)
        {
            // Value was set successfully
        }

        public void onError(SimCapi.Message.GetDataResponse setDataResponse)
        {
            // Error occured value not set
        }
    }

    public void checkComplete(SimCapi.Message.CheckCompleteResponse checkCompleteResponse)
    {
        SimCapiConsole.log("check Complete");
    }

    public void TriggerCheck()
    {
        MyTransporter.triggerCheck(checkComplete);
    }
}

public class FunctionCallbacks
{

}

public class SimCAPIExposeData
{
    // Put your data here
}