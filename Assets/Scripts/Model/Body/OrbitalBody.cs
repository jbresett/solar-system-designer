using UnityEngine;
using UnityEngine.UI;

namespace Planets
{
    public class OrbitalBody : MonoBehaviour
    {

        public string system;

        public string Type
        {
            get { return type; }
            set
            {
                this.type = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private string type;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                gameObject.name = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private new string name;

        public float Mass
        {
            get { return mass; }
            set {
                mass = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private float mass;

        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                Vector3 size = gameObject.transform.localScale;
                float aRadius = radius;
                size.x = aRadius;
                size.y = aRadius;
                size.z = aRadius;
                gameObject.transform.localScale = size;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private float radius;

        public float RevTime
        {
            get { return revTime; }
            set
            {
                revTime = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        public float revTime;

        public Vector3 Vel
        {
            get { return vel; }
            set
            {
                vel = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private Vector3 vel;

        public float BodyRotAxis
        {
            get { return bodyRotAxis; }
            set
            {
                bodyRotAxis = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private float bodyRotAxis;


        public float BodyRotSpd
        {
            get { return bodyRotSpd; }
            set
            {
                bodyRotSpd = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        public float bodyRotSpd;

        public float BodyDampAmt
        {
            get { return bodyDampAmt; }
            set
            {
                bodyDampAmt = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        public float bodyDampAmt;

        public OrbitalBody(string system, string type, string name, float mass, float radius, float revTime, Vector3 vel, float bodyRotAxis, float bodyRotSpd, float bodyDampAmt)
        {
            this.system = system;
            this.type = type;
            
            gameObject.name = name;
            this.name = name;

            this.mass = mass;
            this.revTime = revTime;
            this.vel = vel;
            this.bodyRotAxis = bodyRotAxis;
            this.bodyRotSpd = bodyRotSpd;
            this.bodyDampAmt = bodyDampAmt;
        }


        private void Update()
        {
            Vector3 pos = gameObject.transform.position;
            pos += vel;
            gameObject.transform.position = pos;
            transform.Rotate((Vector3.up * bodyRotSpd) * (Time.deltaTime * bodyDampAmt), Space.Self);
        }

    }
}