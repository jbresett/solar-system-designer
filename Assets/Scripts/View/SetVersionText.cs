using System;
using TMPro;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace DefaultNamespace
{
    public class SetVersionText : MonoBehaviour
    {
        public TextMeshProUGUI aboutText;

        private void setVersion()
        {
            string text = aboutText.text;
            aboutText.text = text.Replace("{version_num}", Sim.Config.Version);
        }

        private void Start()
        {
            setVersion();
        }
    }
}