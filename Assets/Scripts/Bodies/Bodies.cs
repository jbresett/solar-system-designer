using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bodies {
    public const int MAX = 30;

    static private Body[] bodies = new Body[MAX];
         
    /// <summary>
    /// Returns a body by id.
    /// </summary>
    /// <param name="id">Must be between 0 and BODY_COUNT.</param>
    /// <returns></returns>
    static public Body get(int id)
    {
        return bodies[id];
    }

    /// <summary>
    /// Returns a Body by name, or null if the body does not exist.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static public Body get(string name)
    {
        foreach (Body body in bodies)
        {
            if (body != null && body.name.Equals(name)) return body;
        }
        return null;
    }

    static private bool hasInit = false;
    static public void Init(GameObject bodyContainer, GameObject bodyPrefab)
    {
        if (hasInit) return;
        hasInit = true;
        
        // Id pre-existing bodies.
        int i = 0;
        foreach (Body body in Object.FindObjectsOfType<Body>())
        {
            body.Id = i;
            bodies[i] = body;
            i++;
        }

        // Generate new bodies.
        for (  ; i < MAX; i++)
        {
            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //obj.transform.SetParent(bodyContainer.transform);
            GameObject obj = Object.Instantiate(bodyPrefab, bodyContainer.transform);
            obj.name = "";
            obj.SetActive(false);
            Body body = obj.AddComponent<Body>();
            body.Id = i;
            bodies[i] = body;
        }
    }

    static public GameObject add()
    {
        return null;
    }

    /// <summary>
    /// Returns a set of all body values, including empty(null) bodies.
    /// </summary>
    /// <returns></returns>
    static public Body[] getAll()
    {
        return bodies;
    }

    /// <summary>
    /// Returns a list of all active bodies.
    /// </summary>
    /// <returns></returns>
    static public List<Body> getActive()
    {
        List<Body> active = new List<Body>();
        foreach (Body body in bodies)
        {
            if (body != null && body.Active) active.Add(body);
        }
        return active;
    }

    static public GameObject activateNext()
    {
        foreach (Body body in bodies)
        {
            if (body != null && body.gameObject.activeSelf == false) {
                body.gameObject.SetActive(true);
                return body.gameObject;
            }
        }
        return null;
    }

    static public void deactivate(Body body)
    {
        body.gameObject.SetActive(false);
    }
}
