using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collision
    {
        public static bool isCollided(Body b1, Body b2)
        {
            double r1 = b1.Diameter / 2;
            double r2 = b2.Diameter / 2;
            Vector3d p1 = b1.Pos;
            Vector3d p2 = b2.Pos;
            return (r1 + r2) >= calcDist(p1, p2);
        }

        public static double calcDist(Vector3d p1, Vector3d p2)
        {
            double dx = p1.x-p2.x;
            double dy = p1.y-p2.y;
            double dz = p1.z-p2.z;
            return Math.Sqrt((dx * dx) + (dy * dy) + (dz + dz));
        }

        public static void checkCollisions()
        {
            List<Body> bodies = Sim.Bodies.Active;
            for(int i=0;i<bodies.Count;i++)
            {
                Body body1 = bodies[i];
                for (int j = i+1; j < bodies.Count; j++)
                {
                    Body body2 = bodies[j];
                    if (isCollided(body1, body2))
                    {
                        Bodies.deactivate(body1);
                        Bodies.deactivate(body2);
                    }
                }
            }
        }
    }
}