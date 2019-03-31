/// <summary>
/// This class utilizes the laws of orbital mechanics
/// to simulate orbit based on a planet and a stars mass
/// along with their distance from eachother.
///
/// The formulas for this class were obtained from
/// https://www.wired.com/2016/06/way-solve-three-body-problem/
/// https://fiftyexamples.readthedocs.io/en/latest/gravity.html
/// @author Jack Northcutt
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{

    private const double g = 6.67408E-11;

    /// <summary>
    /// This method checks for the body with the most mass and keeps the
    /// velocity for the most mass at zero so that the solar system stays relative
    /// The method then checks each body to see if it has an inital velocity yet or not
    /// and then calculates that bodies initial velocity.
    ///
    /// This method may not account for binary star systems and may not represent the actual starting
    /// velocity of a planet. This method calculates a guess based on mass and distance.
    ///
    /// Partial Equation derrived from:
    /// https://www.physicsclassroom.com/class/circles/Lesson-4/Mathematics-of-Satellite-Motion
    /// </summary>
    public void calcInitialVelocities()
    {
        List<Body> bodyList = Sim.Bodies.Active;
        double xDist = 0;
        Vector3d vec = new Vector3d(0, 0, 0);

        //here we check each body and determine its initial velocity
        //based on the the strongest force applied to the object.
        //we then determine if we should give a star initial velocity
        //or not.
        foreach (Body body in bodyList)
        {
            /// we need to know if initial velocity has already been set
            if (!body.isInitialVel)
            {
                    ///We need the radius, which is the x position, between the body
                    /// and the body it is most attracted too
                    xDist = body.MostPull.Pos.x - body.Pos.x;
                    
                    ///control variable to protect against unwanted retrograde
                    bool negative = false;
                    
                    ///If the distance calculated is less than zero we will
                    /// multiply the xDist by negative 1 to make positive, so
                    /// that we can protect from a divide by zero error.
                    ///
                    /// we set negative to true so we know this value needs to be
                    /// altered after calculation
                    if (xDist < 0)
                    {
                        xDist = xDist * -1;
                        negative = true;
                    }
                    
                    /// working in x/z plane we need to set the z velocity to be
                    /// perpendicular to x
                    vec.z = (Math.Sqrt((g * (body.MostPull.KG)) / (xDist)));
                    
                    /// here is where retrograde is handled.
                    if (!negative)
                    {
                        vec.z = vec.z * -1;
                    }
                    body.Vel = vec;
                    body.isInitialVel = true;
            }
            
        }
    }

    /// <summary>
    /// This method calculates the force of another body on a body
    /// The equation first calculates the the distance between.
    /// Then takes the magnitude of the distance
    /// next force is calculated G * M1 * M2 / (magnitude)^2
    /// we then find the angle theta
    ///
    /// we then use angle theta to calc the direction of the force
    /// </summary>
    /// <param name="body"></param>
    /// <param name="obody"></param>
    /// <returns></returns>
    public Vector3d calcForce(Body body, Body obody)
    {
        Vector3d force;
        Vector3d distance = obody.Pos - body.Pos;
        double distMag = distance.magnitude;

        double aForce = (g * body.KG * obody.KG) / (distMag * distMag);

        double theta = Math.Atan2(distance.z, distance.x);

        force = new Vector3d(Math.Cos(theta) * aForce, 0, Math.Sin(theta) * aForce);

        return force;
    }


    /// <summary>
    /// This method utilizes the above forceCalc method to
    /// sum the force applied on each body by each body.
    /// </summary>
    public void updateForce()
    {
        List<Body> bodies = Sim.Bodies.Active;
        Vector3d force;
        Vector3d aforce = new Vector3d();
        double mostForce = 0;
        //Body mostPull = new Body();

        //We are taking each body and calculating the force between
        //that body and all other bodies, we are also determining
        //which body applies the most force to the specific body.
        foreach (Body bod in bodies)
        {
            force = new Vector3d(0, 0, 0);
            foreach (Body obod in bodies)
            {
                if (bod != obod)
                {
                    aforce = calcForce(bod, obod);
                    if (aforce.magnitude > mostForce)
                    {
                        mostForce = aforce.magnitude;
                        bod.MostPull = obod;

                    }
                    force += aforce;
                }

            }

            mostForce = 0;
            bod.totalForce = force;
        }
    }

    /// <summary>
    ///  This method uses a bodies total force to calculate a new velocity
    /// This function calculates new velocity for each body.
    /// </summary>
    public void updateVelocity()
    {
        updateForce();
        calcInitialVelocities();
        List<Body> bodies = Sim.Bodies.Active;
        Vector3d velocity;
        foreach (Body bod in bodies)
        {
            velocity = bod.Vel;
            velocity.x += bod.totalForce.x / bod.KG * Sim.Settings.Speed;
            velocity.z += bod.totalForce.z / bod.KG * Sim.Settings.Speed;
            velocity.y = bod.Vel.y;
            bod.Vel = velocity;
        }
    }


    /// <summary>
    /// THis function uses the new velocities to calculate a new position
    /// </summary>
    public void calcPosition()
    {
        updateVelocity();
        List<Body> bodies = Sim.Bodies.Active;
        Vector3d newPos;
        foreach (Body bod in bodies)
        {
            newPos = new Vector3d(bod.Pos.x + bod.Vel.x * Sim.Settings.Speed, bod.Pos.y, bod.Pos.z + bod.Vel.z * Sim.Settings.Speed);
            bod.Pos = newPos;
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (!Sim.Settings.Paused)
        {
            calcPosition();
        }
    }

}
