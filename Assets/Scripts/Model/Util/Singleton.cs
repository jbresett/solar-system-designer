// Source: http://wiki.unity3d.com/index.php/Singleton
// Released under the Creative Commons Attribution Share Alike.
// https://creativecommons.org/licenses/by-sa/3.0/
// Changes:
// - Moved "is Shutting Down" check to inside null-check, to allow for active instances 
// within the Editor to be found even when the program is not running.
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {

                        if (m_ShuttingDown)
                        {
                            Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                "' already destroyed. Returning null.");
                            return null;
                        }

                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}