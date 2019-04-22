using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bodies: Singleton<Bodies> {
    public const int MAX_BODY_COUNT = 30;

    private Body[] bodies = new Body[MAX_BODY_COUNT];
         
    /// <summary>
    /// Returns a body by id.
    /// </summary>
    /// <param name="id">Must be between 0 and BODY_COUNT.</param>
    /// <returns></returns>
    public Body get(int id)
    {
        return bodies[id];
    }

    /// <summary>
    /// Returns a Body by name, or null if the body does not exist.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Body get(string name)
    {
        foreach (Body body in bodies)
        {
            if (body != null && body.name.Equals(name)) return body;
        }
        return null;
    }

    public void Awake()
    {
        // Generate the rest of the bodies.
        // All Bodies must exist at initiation due to needing Capi variables.

        // Intial Bodies
        int i = 0;

        Body[] currentBodies = Object.FindObjectsOfType<Body>();

        for (; i < currentBodies.Length; i++)
        {
            currentBodies[i].Init(i);
            bodies[i] = currentBodies[i];
        }

        for ( ; i < MAX_BODY_COUNT; i++)
        {
            GameObject obj = Object.Instantiate(Sim.Config.BodyPrefab, Sim.Config.BodyContainer.transform);
            Body body = obj.GetComponent<Body>();

            body.Init(i);
            body.Name = "Body " + i;

            bodies[i] = body;
        }
    }

    /// <summary>
    /// [Read-Oly]retrives a set of all body values, including empty(null) bodies.
    /// </summary>
    /// <returns></returns>
    public Body[] All
    {
        get { return bodies; }
    }

    /// <summary>
    /// Returns a list of all active bodies.
    /// </summary>
    /// <returns></returns>
    public List<Body> Active 
    {
        get
        {
            List<Body> active = new List<Body>();
            foreach (Body body in bodies)
            {
                if (body != null && body.gameObject.activeSelf)
                {
                    active.Add(body);
                }
            }
            return active;
        }
    }

    [System.Obsolete("Used Sim.Bodies.Active")]
    static public List<Body> getActive()
    {
        return Instance.Active;
    }

    public GameObject activateNext()
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
