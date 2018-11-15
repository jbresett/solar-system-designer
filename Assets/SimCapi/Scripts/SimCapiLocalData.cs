using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace SimCapi
{
    public class SimCapiLocalData
    {
        public SimCapiLocalData()
        {

        }

        public void setData(string simId, string key, string value, SetDataRequestSuccessDelegate successDelegate)
        {
            bool success = ExternalCalls.setKeyPairSessionStorage(simId, key, value);

            SimCapiConsole.log("SimCapiLocalData set data: " + success.ToString());

            if (success == false)
                return;

            Message.SetDataResponse setDataResponse = new Message.SetDataResponse();
            setDataResponse.handshake = Transporter.getInstance().getHandshake();
            setDataResponse.simId = simId;
            setDataResponse.key = key;
            setDataResponse.value = value;
            setDataResponse.responseType = "success";

            if (success == true)
                successDelegate(setDataResponse);
        }



        public void getData(string simId, string key, GetDataRequestSuccessDelegate successDelegate, GetDataRequestErrorDelegate errorDelegate)
        {
            string dataReturnString = ExternalCalls.getKeyPairSessionStorage(simId, key);

            // returns json string defined as below
            /*
                var response =
                {
                    success: true,
                    key: javaKey,
                    value: null,
                    exists: false
                };
            */

            // Parse return string from external call
            JObject jObject = JsonConvert.DeserializeObject<JObject>(dataReturnString);

            JProperty propSuccess = jObject.Property("success");
            //JProperty propKey = jObject.Property("key");
            JProperty propValue = jObject.Property("value");
            JProperty propExists = jObject.Property("exists");

            bool success = propSuccess.Value.ToObject<bool>();


            Message.GetDataResponse getDataResponse = new Message.GetDataResponse();
            getDataResponse.handshake = Transporter.getInstance().getHandshake();
            getDataResponse.simId = simId;
            getDataResponse.key = key;
            getDataResponse.value = null;
            getDataResponse.exists = propExists.ToObject<bool>();

            if (success == true)
                getDataResponse.responseType = "success";
            else
                getDataResponse.responseType = "error";

            if (propValue.Value != null)
                getDataResponse.value = propValue.Value.ToObject<string>();

            if (success == true)
                successDelegate(getDataResponse);
            else
                errorDelegate(getDataResponse);
        }

    }
}