using UnityEngine;
using UnityEngine.Internal.Experimental.UIElements;
using UnityEngine.UI;

namespace Planets
{
    public class SelectBody : MonoBehaviour
    {
        public GameObject selectMenu;
        public GameObject editMenu;
        public GameObject body;
        public Button selectArea;
        public Camera camera;
        private void Start()
        {
            selectArea.onClick.AddListener(select);
        }

        private void select()
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.collider.gameObject.tag == "OrbitalBody")
                {
                    body = hitInfo.collider.gameObject;
                }
            }
            
        }
    }
}