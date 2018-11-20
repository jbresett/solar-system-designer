using UnityEngine;
using UnityEngine.UI;

namespace Planets
{
    public class OrbitalBody : MonoBehaviour
    {

        public Vector3 vel;
        public int mass;
        public int radius;
        public string type;

        public OrbitalBody(Vector3 vel, int mass, int radius)
        {
            this.vel = vel;
            this.mass = mass;
            
            
        }

        public void setRadius(int radius)
        {
            this.radius = radius;
            Vector3 size = gameObject.transform.localScale;
            float aRadius = adjustRadius(radius);
            size.x = aRadius;
            size.y = aRadius;
            size.z = aRadius;
            gameObject.transform.localScale = size;
        }

        private void Update()
        {
            Vector3 pos = gameObject.transform.position;
            pos += vel;
            gameObject.transform.position = pos;
        }

        private float adjustRadius(float radius)
        {
            return radius;
        }
    }
}