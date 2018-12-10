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
                    Text[] comps = selectMenu.GetComponentsInChildren<Text>();
                    foreach (var v in comps)
                    {
                        Debug.Log(v.name);
                    }

                    comps[1].text = body.GetComponent<OrbitalBody>().Type;
                    comps[3].text = body.GetComponent<OrbitalBody>().Name;
                    comps[5].text = body.GetComponent<OrbitalBody>().Vel[0].ToString();
                    comps[7].text = body.GetComponent<OrbitalBody>().Vel[1].ToString();
                    comps[9].text = body.GetComponent<OrbitalBody>().Vel[2].ToString();
                }
            }
            
        }
    }
}