using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRings : MonoBehaviour {

    //Manual Settings
    [Range(3, 360)]
    public int sgmts = 3;
    public float innerR = 0.7f;
    public float ringDepth = 0.5f;
    public Material ringMat;

    //Cached References
    GameObject ring;
    Mesh ringMesh;
    MeshFilter ringMFilter;
    MeshRenderer ringMRenderer;

	// Use this for initialization
	void Start () {

        //Create Ring
        ring = new GameObject(name + " Ring");
        ring.transform.parent = transform;
        ring.transform.localScale = Vector3.one;
        ring.transform.localPosition = Vector3.zero;
        ring.transform.localRotation = Quaternion.identity;
        ringMFilter = ring.AddComponent<MeshFilter>();
        ringMesh = ringMFilter.mesh;
        ringMRenderer = ring.AddComponent<MeshRenderer>();
        ringMRenderer.material = ringMat;

        //Build Ring
        Vector3[] vertices = new Vector3[(sgmts + 1) * 2 * 2];
        int[] triangles = new int[sgmts * 6 * 2];
        Vector2[] uv= new Vector2[(sgmts + 1) * 2* 2];
        int halfRing = (sgmts + 1) * 2;

        for(int i = 0; i < sgmts + 1; i++) {
            float pgrss = (float)i / (float)sgmts;
            float angle = Mathf.Deg2Rad * pgrss * 360;
            float x = Mathf.Sin (angle);
            float z = Mathf.Cos (angle);

            vertices[i * 2] = vertices[i * 2 + halfRing] = new Vector3(x, 0f, z) * (innerR + ringDepth);
            vertices[i * 2 + 1] = vertices[i * 2 + 1 + halfRing] = new Vector3(x, 0f, z) * innerR;
            uv[i * 2] = uv[i * 2 + halfRing] = new Vector2(pgrss, 0f);
            uv[i * 2 + 1] = uv[i * 2 + 1 + halfRing] = new Vector2(pgrss, 1f);

            if(i != sgmts) {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = (i + 1) * 2;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = i * 2 + 1;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2 + halfRing;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = i * 2 + 1 + halfRing;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = (i + 1) * 2 + halfRing;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1 + halfRing;
            }
        }

        ringMesh.vertices = vertices;
        ringMesh.triangles = triangles;
        ringMesh.uv = uv;
        ringMesh.RecalculateNormals();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
