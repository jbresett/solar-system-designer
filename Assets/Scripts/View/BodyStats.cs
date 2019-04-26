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
        public TextMeshProUGUI bodyName;
        public TextMeshProUGUI radius;
        public TextMeshProUGUI mass;
        public TextMeshProUGUI pos;
        public TextMeshProUGUI des;
        public Component viewPort;
        private bool listEmptied = false;

        private void Start()
        {
            bodySelector.onValueChanged.AddListener(delegate { updateView(); });
            populateList();
            updateView();
        }

        private void Update()
        {
            populateList();
            if (bodySelector.options.Count > 0)
            {
                if(listEmptied)
                    updateView();
                listStats();
            }
            else
            {
                viewPort.gameObject.SetActive(false);
            }
        }

        private void populateList()
        {
            Body[] bodies = Sim.Bodies.All;
            List<string> options = new List<string>();
            Body b;
            for (int i = 0; i < bodies.Length; i++)
            {
                b = bodies[i];
                if (!b.name.Equals("") && b.Active)
                {
                    options.Add(bodies[i].name);
                }
            }
            bodySelector.ClearOptions();
            bodySelector.AddOptions(options);
        }

        public void listStats()
        {
            string name = bodySelector.options[bodySelector.value].text;
            if (GameObject.Find(name) != null)
            {
                Body b = Sim.Bodies.get(name);
                bodyName.text = b.Name;
                radius.text = b.Diameter.ToString();
                mass.text = b.Mass.ToString();
                pos.text = b.Pos.ToString();
                if (!b.gameObject.activeSelf) {
                    des.text = "Yes";
                } else {
                    des.text = "No";
                }
                
                viewPort.gameObject.SetActive(false);
                viewPort.gameObject.SetActive(true);
            } else
            {
                Debugger.log("Body No Longer Exists. No Stats Available.");
                updateView();
            }
            
        }

        private void updateView()
        {
            CameraControls.changeBody(bodySelector.options[bodySelector.value].text);
        }
    }
}