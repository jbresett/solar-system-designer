using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class BodyStats : MonoBehaviour
    {
        public Toggle cameraLock;
        public TMP_Dropdown planetSelector;
        public TextMeshProUGUI pos;
        public Component viewPort;

        private void Update()
        {
            Body[] bodies = Sim.Bodies.getAll();
            List<string> options = new List<string>();
            for (int i = 0; i < bodies.Length; i++)
            {
                if (!bodies[i].name.Equals(""))
                {
                    options.Add(bodies[i].name);
                }
            }
            planetSelector.ClearOptions();
            planetSelector.AddOptions(options);
            listStats();
        }

        public void listStats()
        {
            string name = planetSelector.options[planetSelector.value].text;
            Body b = Sim.Bodies.get(name);
            pos.text = b.Pos.ToString();
            viewPort.gameObject.SetActive(false);
            viewPort.gameObject.SetActive(true);
        }
    }
}