using UnityEngine;
using UnityEngine.UI;

namespace Planets
{
    public class OrbitalBody : MonoBehaviour
    {
        public string system;
        public string type;
        public string bodyName;
        public float mass;
        public float radius;
        public float revTime;
        public Vector3 vel;
        public float bodyRotAxis;
        public float bodyRotSpd;
        public float bodyDampAmt;

        public OrbitalBody(string system, string type, string name, float mass, float radius, float revTime, Vector3 vel, float bodyRotAxis, float bodyRotSpd, float bodyDampAmt)
        {
            this.system = system;
            this.type = type;
            this.bodyName = gameObject.name;
            this.mass = mass;
            this.revTime = revTime;
            this.vel = vel;
            this.bodyRotAxis = bodyRotAxis;
            this.bodyRotSpd = bodyRotSpd;
            this.bodyDampAmt = bodyDampAmt;
        }

        public void setRadius(float radius)
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
            transform.Rotate((Vector3.up * bodyRotSpd) * (Time.deltaTime * bodyDampAmt), Space.Self);
        }

        private float adjustRadius(float radius)
        {
            return radius;
        }
    }
}