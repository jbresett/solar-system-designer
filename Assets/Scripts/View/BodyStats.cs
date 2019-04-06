using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BodyStats : MonoBehaviour
    {
        public Toggle cameraLock;
        public TMP_Dropdown bodySelector;
        public TextMeshProUGUI pos;
        public Component viewPort;
        public Body body;
        private void Start()
        {
            bodySelector.onValueChanged.AddListener(delegate { updateView(); });
        }

        private void Update()
        {
            Body[] bodies = Sim.Bodies.getAll();
            List<string> options = new List<string>();
            Body b;
            for (int i = 0; i < bodies.Length; i++)
            {
                b = bodies[i];
                if (!b.name.Equals("")&&b.Active)
                {
                    options.Add(bodies[i].name);
                }
            }
            bodySelector.ClearOptions();
            bodySelector.AddOptions(options);
            if (bodySelector.options.Count > 0)
            {
                listStats();
                
            }
            else
            {
                viewPort.gameObject.SetActive(false);
            }
            
            {
                //Creating a ray that will shoot straight through the scene and
                //and register the first item hit
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                //If object is hit within the first 1000 unity units
                //then we will change the state of body.
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    body = hit.collider.GetComponent<Body>();
                    //setSelected();
                    Debug.Log(body.name);
                }
            }
        }

        public void listStats()
        {
            string name = bodySelector.options[bodySelector.value].text;
            Body b = Sim.Bodies.get(name);
            pos.text = b.Pos.ToString();
            viewPort.gameObject.SetActive(false);
            viewPort.gameObject.SetActive(true);
        }

        private void updateView()
        {
            CameraControls.changeBody(bodySelector.options[bodySelector.value].text);
        }
    }
}