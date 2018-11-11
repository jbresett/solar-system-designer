using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimCapi
{
    public class uuid
    {
        public static string generate()
        {
            string uuid = "";
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (var i = 0; i < 46; i++)
            {
                uuid += chars[Random.Range((int)0, chars.Length)];
            }
            return uuid;
        }

    }
}