using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller.UI
{
    public class RemoveBody : MonoBehaviour
    {
        public TMP_Dropdown body;

        public void removeBody()
        {
            Bodies.deactivate(Sim.Bodies.get(body.options[body.value].text));
        }
    }
}