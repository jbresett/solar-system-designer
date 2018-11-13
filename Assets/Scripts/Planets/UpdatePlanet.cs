using UnityEngine;
using UnityEngine.UI;

namespace Planets
{
    public class UpdatePlanet : MonoBehaviour
    {

        public Vector3 vel;

        public UpdatePlanet(Vector3 vel)
        {
            vel = this.vel;
        }

        private void Update()
        {
            Vector3 pos = gameObject.transform.position;
            pos += vel;
            gameObject.transform.position = pos;
        }
    }
}