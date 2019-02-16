using System;
using TMPro;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace DefaultNamespace
{
    public class SetVersionText : MonoBehaviour
    {
        public TextMeshProUGUI aboutText;
        private string version;

        private void setVersion()
        {
            string text = aboutText.text;
            aboutText.text = text.Replace("{version_num}", version);
        }

        private void Start()
        {
            version = Sim.Config.Version;
            setVersion();
        }
    }
}