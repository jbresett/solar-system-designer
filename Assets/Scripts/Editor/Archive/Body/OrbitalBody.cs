using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class defines an orbital body that is placed in an
/// n-body system collection
/// </summary>
namespace Planets
{
    public class OrbitalBody : MonoBehaviour
    {

        public string system;

        /// <summary>
        /// This function gets and sets the type of body defined
        /// </summary>
        public string Type
        {
            get { return type; }
            set
            {
                this.type = value;
               
            }
        }
        [SerializeField]
        private string type;

        /// <summary>
        /// This function gets and sets the name of the defined body
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                gameObject.name = value;
               
            }
        }
        [SerializeField]
        private new string name;

        /// <summary>
        /// This function gets and sets the mass of the body defined
        /// </summary>
        public double Mass
        {
            get { return mass; }
            set {
                mass = value;
                
            }
        }
        [SerializeField]
        private double mass;

        /// <summary>
        /// This function gets and sets the radius of the body defined
        /// </summary>
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
    
            }
        }
        [SerializeField]
        private double radius;

        /// <summary>
        /// This function gets and sets the revolution time of the body defined
        /// </summary>
        public float RevTime
        {
            get { return revTime; }
            set
            {
                revTime = value;
        
            }
        }
        [SerializeField]
        public float revTime;

        /// <summary>
        /// This function gets and sets the position of the body defined
        /// </summary>
        public Vector3d Pos
        {
            get { return pos; }
            set
            {
                pos = value;
                //Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private Vector3d pos;

        /// <summary>
        /// This function gets and sets the velocity of the body defined
        /// </summary>
        public Vector3d Vel
        {
            get { return vel; }
            set
            {
                vel = value;
 
            }
        }
        [SerializeField]
        private Vector3d vel;

        /// <summary>
        /// This function gets and sets the rotation axis of the body defined
        /// </summary>
        public double BodyRotAxis
        {
            get { return bodyRotAxis; }
            set
            {
                bodyRotAxis = value;
      
            }
        }
        [SerializeField]
        private double bodyRotAxis;

        /// <summary>
        /// This function gets and sets the rotation speed of the body defined
        /// </summary>
        public double BodyRotSpd
        {
            get { return bodyRotSpd; }
            set
            {
                bodyRotSpd = value;

            }
        }
        [SerializeField]
        public double bodyRotSpd;

        /// <summary>
        /// This function gets and sets the body Damp Amount of the body defined
        /// </summary>
        public double BodyDampAmt
        {
            get { return bodyDampAmt; }
            set
            {
                bodyDampAmt = value;

            }
        }
        [SerializeField]
        public double bodyDampAmt;

        /// <summary>
        /// This function is the constructor for the class
        /// </summary>
        public OrbitalBody(string system, string type, string name, double mass, float radius, float revTime, Vector3d pos, Vector3d vel, float bodyRotAxis, float bodyRotSpd, float bodyDampAmt)
        {
            this.system = system;
            this.type = type;
            
            gameObject.name = name;
            this.name = name;

            this.mass = mass;
            this.revTime = revTime;
            this.pos = pos;
            this.vel = vel;
            this.bodyRotAxis = bodyRotAxis;
            this.bodyRotSpd = bodyRotSpd;
            this.bodyDampAmt = bodyDampAmt;
        }

        /// <summary>
        /// This function updates once per frame
        /// </summary>
        private void Update()
        {
            /* Motion handled by seperate Rotation script
            Vector3 pos = gameObject.transform.position;
            pos += vel.Vec3;
            gameObject.transform.position = pos;
            transform.Rotate((Vector3.up * (float)bodyRotSpd) * (Time.deltaTime * (float)bodyDampAmt), Space.Self);
            */
        }

        private void Start()
        {
            
        }
    }
}