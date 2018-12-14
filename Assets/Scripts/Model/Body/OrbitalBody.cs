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

        public double Mass
        {
            get { return mass; }
            set {
                mass = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private double mass;

        public double Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                Vector3 size = gameObject.transform.localScale;
                double aRadius = radius;
                size.x = (float)aRadius;
                size.y = (float)aRadius;
                size.z = (float)aRadius;
                gameObject.transform.localScale = size;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private double radius;

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

        public Vector3d Vel
        {
            get { return vel; }
            set
            {
                vel = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private Vector3d vel;

        public double BodyRotAxis
        {
            get { return bodyRotAxis; }
            set
            {
                bodyRotAxis = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private double bodyRotAxis;


        public double BodyRotSpd
        {
            get { return bodyRotSpd; }
            set
            {
                bodyRotSpd = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        public double bodyRotSpd;

        public double BodyDampAmt
        {
            get { return bodyDampAmt; }
            set
            {
                bodyDampAmt = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        public double bodyDampAmt;

        public OrbitalBody(string system, string type, string name, double mass, float radius, float revTime, Vector3d vel, float bodyRotAxis, float bodyRotSpd, float bodyDampAmt)
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
            /* Motion handled by seperate Rotation script
            Vector3 pos = gameObject.transform.position;
            pos += vel.Vec3;
            gameObject.transform.position = pos;
            transform.Rotate((Vector3.up * (float)bodyRotSpd) * (Time.deltaTime * (float)bodyDampAmt), Space.Self);
            */
        }

    }
}