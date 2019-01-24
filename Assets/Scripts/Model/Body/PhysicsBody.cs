using System;
using SimCapi;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class defines handles all of the physics for the
/// celestial objects supplied by the OrbitalBody clas.
/// </summary>
namespace Planets
{
    public enum BodyType
    {
        Star, Planet, Moon, Astroid
    }

    public class PhysicsBody : MonoBehaviour
    {
        /// <summary>
        /// Returns the unique Id / Index Number of Nbody.
        /// </summary>
        public int Id { get { return id; } }
        [SerializeField]
        private int id = -1;

        public BodyType Type
        {
            get { return type; }
            set {
                type = value;
                capiType.setValue(value);
            }
        }
        [SerializeField]
        private BodyType type;
        private SimCapiEnum<BodyType> capiType;

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
                capiName.setValue(value);
            }
        }
        //[SerializeField] "name" not needed: Name part of the MonoBehavior.
        //private string name;
        private SimCapiString capiName;

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
                capiMass.setValue((float)value);
            }
        }
        /// <summary>
        /// Mass of the body in Suns.
        /// </summary>
        public double SolarMass
        {
            get { return mass / SOLAR_MASS_CONVERT; }
            set { Mass = value / SOLAR_MASS_CONVERT; }
        }
        /// <summary>
        /// Mass of the Body in kg.
        /// </summary>
        public double KG
        {
            get { return mass * KG_MASS_CONVERT; }
            set { Mass = value / KG_MASS_CONVERT; }
        }
        // Mass stored in Earth masses.
        [SerializeField]
        private double mass;
        private SimCapiNumber capiMass;

        /// <summary>
        /// Diameter in Earths.
        /// </summary>
        public double Diameter
        {
            get { return diameter; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Diameter must be greater than 0.");
                diameter = value;
                capiDiameter.setValue((float)value);
                Vector3 size = gameObject.transform.localScale;
                float unityDiamter = (float)(Diameter * DIAMETER_MULT);
                size.x = unityDiamter;
                size.y = unityDiamter;
                size.z = unityDiamter;
                gameObject.transform.localScale = size;
            }
        }
        [SerializeField]
        private double diameter;
        private SimCapiNumber capiDiameter;

        /// <summary>
        /// Current Position of the object. Any changes will be reflected in Unity.
        /// </summary>
        public Vector3d Position
        {
            get { return position; }
            set
            {
                position = value;
                capiPosition.getList().Clear();
                capiPosition.getList().Add(value.x.ToString());
                capiPosition.getList().Add(value.y.ToString());
                capiPosition.getList().Add(value.z.ToString());
                capiPosition.updateValue();
                gameObject.transform.position = (value * DISTANCE_MULT).Vec3;
            }
        }
        [SerializeField]
        private Vector3d position;
        private SimCapiStringArray capiPosition;

        /// <summary>
        /// Initial position of the Body. Using resetPosition() will move the Body back to it's initial position.
        /// </summary>
        public Vector3d InitialPosition
        {
            get { return inititialPosition; }
            set
            {
                inititialPosition = value;
                capiInitialPosition.getList().Clear();
                capiInitialPosition.getList().Add(value.x.ToString());
                capiInitialPosition.getList().Add(value.y.ToString());
                capiInitialPosition.getList().Add(value.z.ToString());
                capiInitialPosition.updateValue();
            }
        }
        [SerializeField]
        private Vector3d inititialPosition;
        private SimCapiStringArray capiInitialPosition;

        /// <summary>
        /// Planet's rotational speed, in earth days.
        /// </summary>
        public double Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                capiRotation.setValue((float)value);
            }
        }
        [SerializeField]
        private double rotation;
        private SimCapiNumber capiRotation;

        /// <summary>
        /// Creates a PhysicsBody on the x and y axis.
        /// </summary>
        /// <param name="name">Body's name.</param>
        /// <param name="zPosition">Position and initial position of (0,0,zPosition), in AU.</param>
        /// <param name="mass">Multiple of Solar Mass (Star BodyType) or Earth's Mass (other types).</param>
        /// <param name="diameter">Multiple of Earth's diameter.</param>
        /// <param name="rotation">Rotation time in days.</param>
        public void setAll(string name, BodyType type, double zPosition, double mass, double diameter, double rotation)
        {
            setAll(name, type, new Vector3d(0, 0, zPosition), mass, diameter, rotation);
        }

        /// <summary>
        /// Creates an PhysicsBody.
        /// </summary>
        /// <param name="name">Body's name.</param>
        /// <param name="type">Body Type.</param>
        /// <param name="position">Position and initial position in AU.</param>
        /// <param name="mass">Multiple of Solar Mass (Star BodyType) or Earth's Mass (other types).</param>
        /// <param name="diameter">Multiple of Earth's diameter.</param>
        /// <param name="rotation">Rotation time in days.</param>
        public void setAll(string name, BodyType type, Vector3d position, double mass, double diameter, double rotation)
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
            Position = inititialPosition;
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
        /// Gets interacting force between this body and another.
        /// Resultsa are in: [N * AU^2 / EM^2] (AU = Astraomical Units, EM = earth Masses).
        /// </summary>
        /// <param name="withBody"></param>
        /// <returns></returns>
        public double force(PhysicsBody withBody)
        {
            return Mathd.G * mass * withBody.mass / Vector3d.DistanceSqr(position, withBody.Position);
        }

        /// <summary>
        /// This method uses the gravity class to calculate the position of an object.
        /// </summary>
        /// <param name="withBody"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Vector3d getPosition(PhysicsBody withBody, double distance){
            Gravity grav = new Gravity(withBody.KG, this.KG, distance);
            return grav.calcPosition();
        }


        private void Awake()
        {
            // Get Id
            id = NBody.register(this);

            // Create Capi values and expose.
            capiName = new SimCapiString(name);
            capiName.expose(id + " Name", false, false);

            capiType = new SimCapiEnum<BodyType>(type);
            capiType.expose(id + " Type", false, false);

            capiPosition = new SimCapiStringArray();
            capiPosition.expose(id + " Position", false, false);

            capiInitialPosition = new SimCapiStringArray();
            capiInitialPosition.expose(id + " InitialPosition", false, false);

            capiMass = new SimCapiNumber((float)mass);
            capiMass.expose(id + " Mass", false, false);

            capiDiameter = new SimCapiNumber((float)diameter);
            capiDiameter.expose(id + " Diameter", false, false);

            capiRotation = new SimCapiNumber((float)rotation);
            capiRotation.expose(id + " Rotation", false, false);


            // Set Deligates

            capiName.setChangeDelegate(
                delegate (string value, SimCapi.ChangedBy changedBy)
                {
                    name = value;
                }
            );
            capiType.setChangeDelegate(
                delegate (BodyType value, SimCapi.ChangedBy changedBy)
                {
                    type = value;
                }
            );

            capiPosition.setChangeDelegate(
                delegate (string[] values, SimCapi.ChangedBy changedBy)
                {
                    Position.x = Convert.ToDouble(values[0]);
                    Position.y = Convert.ToDouble(values[1]);
                    Position.z = Convert.ToDouble(values[2]);
                }
             );

            capiInitialPosition.setChangeDelegate(
                delegate (string[] values, SimCapi.ChangedBy changedBy)
                { 
                    InitialPosition.x = Convert.ToDouble(values[0]);
                    InitialPosition.y = Convert.ToDouble(values[1]);
                    InitialPosition.z = Convert.ToDouble(values[2]);
                }             
            );

            capiMass.setChangeDelegate(
                delegate (float value, SimCapi.ChangedBy changeBy)
                {
                    Mass = value;
                }
            );

            capiDiameter.setChangeDelegate(
                delegate (float value, SimCapi.ChangedBy changeBy)
                {
                    Diameter = value;
                }
             );

            capiRotation.setChangeDelegate(
                delegate (float value, SimCapi.ChangedBy changeBy)
                {
                    Rotation = value;
                }
             );
        }

        public void OnDestroy()
        {
            NBody.unregister(this);

        }

        /// <summary>
        /// This function updates once per frame.
        /// </summary>
        private void Update()
        {

        }

    }
}