using UnityEngine;
using UnityEngine.UI;

namespace Planets
{
    public class OrbitalBody : MonoBehaviour
    {

        public double[] vel;
        public double mass;
        public double radius;
        public string type;
        private double[] pos;

        public void setRadius(double radius)
        {
            this.radius = radius;
            Vector3 size = gameObject.transform.localScale;
            size.x = 2*(float)radius;
            size.y = 2*(float)radius;
            size.z = 2*(float)radius;
            gameObject.transform.localScale = size;
        }

        public void setPos(double[] pos)
        {
            this.pos = pos;
            updatePos();
        }
        public void setVel(double[] vel)
        {
            this.vel = vel;
            
        }

        private void updatePos()
        {
            Vector3 newPos = new Vector3((float)pos[0],(float)pos[1],(float)pos[2]);
            gameObject.transform.position = newPos;
        }

        public void setMass(double mass)
        {
            this.mass = mass;
        }
        private void Update()
        {
            for (int i = 0; i < 3; i++)
            {
                pos[i] += vel[i];
            }
            updatePos();
        }
    }
}