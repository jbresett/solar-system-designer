using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimCapi
{

    [System.Serializable]
    public class SimCapiUserData
    {
        public string id;
        public string givenName;
        public string surname;

        public SimCapiUserData()
        {

        }
    }

    [System.Serializable]
    public class SimCapiConfig
    {
        public string servicesBaseUrl;
        public string context;
        public SimCapiUserData userData;
        public string lessonId;
        public string questionId;
        public int lessonAttempt;

        public SimCapiConfig()
        {

        }
    }
}