using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimCapi
{
    public class SimCapiHandshake
    {
        public string requestToken;
        public string authToken;
        public string version;
        public SimCapiConfig config;

        public static SimCapiHandshake create(string requestToken, string authToken, string version)
        {
            SimCapiHandshake handshake = new SimCapiHandshake();
            handshake.requestToken = requestToken;
            handshake.authToken = authToken;
            handshake.version = version;
            return handshake;
        }
    }
}