using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimCapi
{
    public static class ExternalCalls
    {

#if UNITY_EDITOR

        static bool _isInIFrame;
        static bool _isInAuthor;

        public static void resetState()
        {
            _isInIFrame = false;
            _isInAuthor = false;
        }

        public static void setIsInIFrame(bool state)
        {
            _isInIFrame = state;
        }

        public static void setIsInAuthor(bool state)
        {
            _isInAuthor = state;
        }

        public static void postMessage(string str)
        {

        }

        public static void bindMessageListener(string receiverName)
        {

        }

        public static bool isInIFrame()
        {
            return _isInIFrame;
        }

        public static bool isInAuthor()
        {
            return _isInAuthor;
        }

        public static bool setKeyPairSessionStorage(string simId, string key, string value)
        {
            return false;
        }

        public class TempReturn
        {
            public bool success;
            public string key;
            public string value;
            public bool exists;

            public TempReturn()
            {
                success = true;
                key = "invalid";
                value = null;
                exists = false;
            }
        }

        public static string getKeyPairSessionStorage(string simId, string key)
        {
            TempReturn tempReturn = new TempReturn();
            tempReturn.key = key;
            return JsonUtility.ToJson(tempReturn);
        }

        public static string getDomain()
        {
            return "INVALID_DOMAIN";
        }

        public static void setDomain(string newDomain)
        {

        }

#else

        public static void postMessage(string str)
        {
            SimCapiJavaBindings.postMessage(str);
        }

        public static void bindMessageListener(string receiverName)
        {
            SimCapiJavaBindings.bindMessageListener(receiverName);
        }

        public static bool isInIFrame()
        {
            return SimCapiJavaBindings.isInIFrame();
        }

        public static bool isInAuthor()
        {
            return SimCapiJavaBindings.isInAuthor();
        }

        public static bool setKeyPairSessionStorage(string simId, string key, string value)
        {
            return SimCapiJavaBindings.setKeyPairSessionStorage(simId, key, value);
        }

        public static string getKeyPairSessionStorage(string simId, string key)
        {
            return SimCapiJavaBindings.getKeyPairSessionStorage(simId, key);
        }

        public static string getDomain()
        {
            return SimCapiJavaBindings.getDomain();
        }

        public static void setDomain(string newDomain)
        {
            SimCapiJavaBindings.setDomain(newDomain);
        }

#endif


    }
}