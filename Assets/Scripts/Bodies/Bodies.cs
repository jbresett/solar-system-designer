using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bodies : MonoBehaviour {
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

    static public void add(Body body)
    {
        for (int i = 0; i < MAX; i++)
        {
            if (bodies[i] == null)
            {
                bodies[i] = body;
                body.Id = i;
                return;
            }
        }
        Debugger.log("Too many bodies added.");
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


    private void Awake()
    {
        
    }
}
