using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class MouseControls : MonoBehaviour
{
    public bool mouseEnabled = true;
    public float dragSpeed = 2;
    private Vector3 dragOrigin;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseEnabled)
        {
            dragOrigin = Input.mousePosition;
            return;
        }
 
        if (!Input.GetMouseButton(0)) return;
 
        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        float x = pos.x;
        float y = pos.y;

        pos.x = y;
        pos.y = -x;
 
        transform.Rotate(pos);
    }
}