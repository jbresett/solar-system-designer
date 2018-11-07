using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;

/*
namespace SimCapi
{

    public class APIMethodParameters
    {
        public APIMethodParameters()
        {

        }
    }

    public class API_CALL_REQUEST_Message
    {
        public SimCapiMessageType type;
        public SimCapiHandshake handshake;
        public string api;
        public string method;
        public string uid;
        public APIMethodParameters apiMethodParameters;

        public API_CALL_REQUEST_Message(SimCapiHandshake handshake, string api, string method, string uid, APIMethodParameters apiMethodParameters)
        {
            type = SimCapiMessageType.API_CALL_REQUEST;
            this.handshake = handshake;
            this.api = api;
            this.method = method;
            this.uid = uid;
            this.apiMethodParameters = apiMethodParameters;
        }
    }


    public class SimCapiAPIInterface
    {
        static SimCapiAPIInterface _singleton;

        Dictionary<string, string[]> _apiDictionary;
        Dictionary<string, System.Delegate> _callbackMap;

        Transporter _transporter;
        int _api_call_uid;

        public static SimCapiAPIInterface getInstance()
        {
            if (_singleton == null)
                _singleton = new SimCapiAPIInterface(Transporter.getInstance());

            return _singleton;
        }

        public SimCapiAPIInterface(Transporter transporter)
        {
            _transporter = transporter;

            _api_call_uid = 0;
            _apiDictionary = new Dictionary<string, string[]>();
            _callbackMap = new Dictionary<string, System.Delegate>();

            _apiDictionary["ChemicalAPI"] = new string[] { "getStructure", "search" };
            _apiDictionary["DeviceAPI"] = new string[] { "listDevicesInGroup" };
            _apiDictionary["DataSyncAPI"] = new string[] { "createSession", "joinSession", "setSessionData", "getSessionData" };
            _apiDictionary["InchRepoService"] = new string[] { "search" };
            _apiDictionary["PeerResponseAPI"] = new string[] { "getPeerIds", "getResponses" };
        }

        string[] getAPIMethodArray(string apiName)
        {
            foreach (KeyValuePair<string, string[]> keyValuePair in _apiDictionary)
            {
                if (keyValuePair.Key == apiName)
                    return keyValuePair.Value;
            }

            return null;
        }

        public void apiCall(string apiName, string apiMethod, APIMethodParameters apiMethodParameters, System.Delegate callback)
        {
            
            string[] methodNameArray = getAPIMethodArray(apiName);

            if (methodNameArray == null)
            {
                // Api does not exist
                Debug.LogError("\"" + apiName + "\" is not a valid API name in function api_call in API_Interface of the simcapi unity plugin");
                return;
            }

            if (methodNameArray.Contains(apiMethod) == false)
            {
                // Method does not exist
                Debug.LogError("\"" + apiMethod + "\" is not a valid function in the " + apiName + " api for the simcapi unity plugin");
                return;
            }

            // Get a unique id for the api call

            string uid_string = _api_call_uid.ToString();
            _api_call_uid++;


            //TODO: finish implementing this function

            
            string json_string = SimCapiJsonMaker.create_API_CALL_REQUEST(_transporter.getHandshake(), apiName, apiMethod, uid_string, apiMethodParameters);
            
            // Add to callback map
            if (callback != null)
                _callbackMap[uid_string] = callback;
            
        }



        public void processApiResponse(string json)
        {

        }
    }
}
*/