using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make Insert particles area based on parent Game Object, and removes the
/// object onces the graphics are complete.
/// </summary>
public class InsertParticle : MonoBehaviour {
    // Starting Minimal size for the InsertParticleSystem.
    const float MIN_SIZE = 20;

    // Time in seconds before Particle Object is removed.
    const float KEEP_ALIVE_TIME = 3.5F;

    /// <summary>
    ///  
    /// Defines the minimal size of the burst. Used for smaller bodies to keep
    /// visualization working. 
    /// </summary>
    public Vector3 MinimumSize;

    // Game Object destroy time.
    private float endTime;
    

	void Start () {
        // Sets localScale to greater of parent-scale or minimum size.
        Transform parent = transform.parent;
        transform.localScale = Vector3.Max(parent.localScale, MinimumSize);

        // Set Game Object destroy time to start time + #.
        endTime = Time.time + KEEP_ALIVE_TIME;
	}

	void Update () {
        
        // Destroy game object on time out.
        if (Time.time >= endTime)
        {
            Destroy(gameObject);
        }
        	
	}
}
