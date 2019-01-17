using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class defines an orbital body that is placed in an
/// n-body system collection
/// </summary>
namespace Planets
{
    public enum BodyType
    {
        Star, Planet, Moon, Astroid
    }

    public class PhysicsBody : MonoBehaviour
    {
        public BodyType Type
        {
            get { return Type; }
            set { type = value; }
        }
        [SerializeField]
        private BodyType type;

        // multiplers for Unity Scenes for distances and diameters.
        private static double DISTANCE_MULT = 1500;
        private static double DIAMETER_MULT = 5;

        private static double SOLAR_MASS_CONVERT = 334672.021419; // Solar Mass in Earths.
        private static double KG_MASS_CONVERT = 5.9722E24; // Earth's Mass in KG.

        /// <summary>
        /// Body name. Should be unique for it's system.
        /// </summary>
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

        /// <summary>
        /// Mass of the body in Earths.
        /// </summary>
        public double Mass
        {
            get { return mass; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Mass must be greater than 0.");
                mass = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        /// <summary>
        /// Mass of the body in Suns.
        /// </summary>
        public double SolarMass
        {
            get { return mass / SOLAR_MASS_CONVERT; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Mass must be greater than 0.");
                mass = value / SOLAR_MASS_CONVERT;
            }
        }
        /// <summary>
        /// Mass of the Body in kg.
        /// </summary>
        public double KG
        {
            get { return mass * KG_MASS_CONVERT; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Mass must be greater than 0.");
                mass = value / KG_MASS_CONVERT;
            }
        }
        // Mass stored in Earth masses.
        [SerializeField]
        private double mass;

        /// <summary>
        /// Diameter in Earths.
        /// </summary>
        public double Diameter
        {
            get { return diameter; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Diameter must be greater than 0.");
                this.diameter = value;
                Vector3 size = gameObject.transform.localScale;
                float unityDiamter = (float)(Diameter * DIAMETER_MULT);
                size.x = unityDiamter;
                size.y = unityDiamter;
                size.z = unityDiamter;
                gameObject.transform.localScale = size;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private double diameter;

        /// <summary>
        /// Current Position of the object. Any changes will be reflected in Unity.
        /// </summary>
        public Vector3d Position
        {
            get { return position; }
            set
            {
                position = value;
                gameObject.transform.position = value.Vec3;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private Vector3d position;

        /// <summary>
        /// Initial position of the Body. Using resetPosition() will move the Body back to it's initial position.
        /// </summary>
        public Vector3d InitialPosition
        {
            get { return position; }
            set
            {
                inititialPosition = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private Vector3d inititialPosition;

        /// <summary>
        /// Planet's rotational speed, in earth days.
        /// </summary>
        public double Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                Main.Instance.Exposed.BodyUpdate(this);
            }
        }
        [SerializeField]
        private double rotation;

        /// <summary>
        /// Creates a PhysicsBody on the x and y axis.
        /// </summary>
        /// <param name="name">Body's name.</param>
        /// <param name="zPosition">Position and initial position of (0,0,zPosition), in AU.</param>
        /// <param name="mass">Multiple of Solar Mass (Star BodyType) or Earth's Mass (other types).</param>
        /// <param name="diameter">Multiple of Earth's diameter.</param>
        /// <param name="rotation">Rotation time in days.</param>
        public PhysicsBody(string name, BodyType type, double zPosition, double mass, double diameter, double rotation) :
                this(name, type, new Vector3d(0, 0, zPosition), mass, diameter, rotation)
        { }

        /// <summary>
        /// Creates an PhysicsBody.
        /// </summary>
        /// <param name="name">Body's name.</param>
        /// <param name="type">Body Type.</param>
        /// <param name="position">Position and initial position in AU.</param>
        /// <param name="mass">Multiple of Solar Mass (Star BodyType) or Earth's Mass (other types).</param>
        /// <param name="diameter">Multiple of Earth's diameter.</param>
        /// <param name="rotation">Rotation time in days.</param>
        public PhysicsBody(string name, BodyType type, Vector3d position, double mass, double diameter, double rotation)
        {
            Name = name;
            Type = type;

            Position = position;
            InitialPosition = position;

            if (type == BodyType.Star)
            {
                Mass = mass;
            }
            else
            {
                SolarMass = mass;
            }
            Diameter = diameter;
            Rotation = rotation;
        }
        
        // Resets the body to it's initial position.
        public void resetPosition()
        {
            Position = InitialPosition;
        }

        /// <summary>
        /// Calculates the Barycenter between two objects.
        /// </summary>
        /// <param name="withBody">2nd Objec</param>
        /// <returns>Vector3d representing the Barycenter.</returns>
        public Vector3d getBaycenter(PhysicsBody withBody)
        {
            // distance to BaryCenter.
            double baryDistance = (Vector3d.Distance(position, withBody.Position) * withBody.Mass) / (Mass + withBody.Mass);

            return Vector3d.LerpUnclamped(position, withBody.Position, baryDistance);
        }

        /// <summary>
        /// This function updates once per frame.
        /// </summary>
        private void Update()
        {

        }

    }
}