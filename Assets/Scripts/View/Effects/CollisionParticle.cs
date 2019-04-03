using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make Insert particles area based on parent Game Object, and removes the
/// object onces the graphics are complete.
/// </summary>
public class CollisionParticle : MonoBehaviour {
    // Starting Minimal size for the InsertParticleSystem.
    const float MIN_SIZE = 1;

    /// <summary>
    ///  
    /// Defines the minimal size of the burst. Used for smaller bodies to keep
    /// visualization working. 
    /// </summary>
    public Vector3 MinimumSize = new Vector3(MIN_SIZE, MIN_SIZE, MIN_SIZE);

    // Game Object destroy time.
    private float endTime;
    

	void Start () {
        // Sets localScale to greater of parent-scale or minimum size.
        transform.localScale = Vector3.Max(transform.parent.localScale, MinimumSize);
	}

	void Update () {
 
	}
}
