using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bodies: Singleton<Bodies> {
    public const int MAX_BODY_COUNT = 30;

    int nextBodyId = 0;
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
        
        // Id pre-existing bodies.
        int i = 0;
        foreach (Body body in Object.FindObjectsOfType<Body>())
        {

            bodies[i] = body;
            i++;
        }

        // Generate new bodies.
        for (  ; i < MAX_BODY_COUNT; i++)
        {
            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //obj.transform.SetParent(bodyContainer.transform);
            GameObject obj = Object.Instantiate(Sim.Config.BodyPrefab, Sim.Config.BodyContainer.transform);
            obj.name = "";
            obj.SetActive(false);

            Body body = obj.GetComponent<Body>();
            bodies[i] = body;
        }
    }

    /// <summary>
    /// Called by CapiBody.Awake();, added Body to list and sets it's id.
    /// </summary>
    /// <param name="obj"></param>
    public void add(GameObject obj)
    {
        Body body = obj.GetComponent<Body>();
        bodies[nextBodyId] = body;
        body.Id = nextBodyId;
        nextBodyId++;
    }

    /// <summary>
    /// Returns a set of all body values, including empty(null) bodies.
    /// </summary>
    /// <returns></returns>
    public Body[] getAll()
    {
        return bodies;
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
