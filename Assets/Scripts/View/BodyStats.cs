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
        public TextMeshProUGUI name;
        public TextMeshProUGUI radius;
        public TextMeshProUGUI mass;
        public TextMeshProUGUI pos;
        public Component viewPort;

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
                if (!b.name.Equals("") && b.Active)
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
        }

        public void listStats()
        {
            string name = bodySelector.options[bodySelector.value].text;
            Body b = Sim.Bodies.get(name);
            name.text = b.Name.ToString();
            radius.text = b.Radius.ToString();
            mass.text = b.Mass.ToString();
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