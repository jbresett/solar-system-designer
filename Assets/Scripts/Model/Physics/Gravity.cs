/// <summary>
///
/// This class defines the gravitational pull of all bodies within the simulation, which
/// determines a bodies overall orbit or movement through space. This class extends
/// MonoBehavior and updates the position of the bodies in the update function.
///
/// How the class operates:
/// 1.  update() calls calcPosition()
/// 2.  calcPosition() calls updateVelocity()
/// 3.  updateVelocity calls updateForce()
/// 4.  updateForce() calls calcForce() to calculate the force applied to each body for each body
/// 5.  updateForce() stores the total force for each body within each body object
/// 6.  Returns to updateVelocity() and calcInitialVelocity is called.
/// 7.  calcInitialVelocity() checks each body to see if initial velocity has not been set then sets it.
/// 8.  Returns to updateVelocity() and now that force and an initial velocity are established, acceleration
///         is calculated, which with kinematics allows us to compute a new velocity.
/// 9.  Returns to calcPosition() and a new position is calculated with the new velocity
/// 10. Repeats
///
/// Links that aided in the development of this class and derived equations
/// and may help with future development:
/// 
/// https://www.phyley.com/find-force-given-xy-components
/// https://www.physicsclassroom.com/class/circles/Lesson-4/Mathematics-of-Satellite-Motion
/// https://www.wired.com/2016/06/way-solve-three-body-problem/
/// https://fiftyexamples.readthedocs.io/en/latest/gravity.html
/// https://www.physicsclassroom.com/class/1DKin/Lesson-6/Kinematic-Equations
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
    /// The method then checks each body to see if it has an initial velocity yet or not
    /// and then calculates that bodies initial velocity.
    ///
    /// This method may not account for binary star systems and may not represent the actual starting
    /// velocity of a planet. This method calculates a guess based on mass and distance.
    /// </summary>
    public void calcInitialVelocities()
    {
        // list of all active bodies in the simulation
        List<Body> bodyList = Sim.Bodies.Active;
        
        // This is the x position distance of a body from another body, which describes how far away
        // a body is from another body if you were to draw a line between them
        double xDist = 0;
        
        // Initializing our vector to add a new velocity to a body.
        Vector3d vec = new Vector3d(0, 0, 0);

        // looping through all active bodies to determine if initial velocity for that body needs to be set
        // or not
        foreach (Body body in bodyList)
        {
            // we need to know if initial velocity has already been set, Initial velocity for a body is set when a 
            // user enters there own initial velocity or after executing this if statement once.
            if (!body.isInitialVel)
            {
                // For the auto generation of initial velocities using Kepler's third law, we require there be at
                // least 2 bodies active in the simulation.
                if (bodyList.Count > 1)
                {
                    // Now we need to determine the x distance, by subtracting the body's x distance to the body
                    // it is most attracted to, which is explained and determined in the updateForce() function.
                    xDist = body.MostPull.Pos.x - body.Pos.x;

                    // In nature it is uncommon to have retrograde orbits, which means all bodies are orbiting in
                    // one natural direction and the retrograde is a body going in the opposite direction, so for
                    // an auto generated initial velocity, we want to avoid this retrograde orbit. Through the 
                    // calculation of distance, we can have a positive or negative number, so this is just a control
                    // variable to check for that.
                    bool negative = false;

                    //If the distance calculated is less than zero we will
                    // multiply the xDist by negative 1 to make positive, so
                    // that we can protect from a divide by zero error.
                    //
                    // we set negative to true so we know this value needs to be
                    // altered after calculation
                    if (xDist < 0)
                    {
                        xDist = xDist * -1;
                        negative = true;
                    }

                    // For our initial velocity equations, we are only giving it a z direction for initial
                    // velocity, once in the updateVelocity() function, both x and z velocity will be updated in
                    // accordance with the body and force applied. 
                    // Equation used: initial velocity = sqrt((gravity * Mass of large body)/distance))
                    vec.z = (Math.Sqrt((g * (body.MostPull.KG)) / (xDist)));

                    // here is where retrograde is handled.
                    if (!negative)
                    {
                        vec.z = vec.z * -1;
                    }

                    body.Vel = vec;
                }
        
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
    public Vector3d calcForce(Body currentBody, Body pullingBody)
    {
        // This vector will be used to store the force applied
        // to the body
        Vector3d force;
        
        // For the gravitational force equation we need to know the distance
        // of the body to the other body and since we have a 3 dimensional position
        // we need to take the magnitude of the distance to yield a single double precision
        // number.  Magnitude = sqrt(x^2 + y^2 + z^2)
        Vector3d distance = pullingBody.Pos - currentBody.Pos;
        double distMag = distance.magnitude;
        
        // As per the gravitational force equation
        // force = (gravity * mass1 * mass2)/ distance^2
        double aForce = (g * currentBody.KG * pullingBody.KG) / (distMag * distMag);
        
        // We know that the force is not a straight pull on eachother, so we need to find the direction of the force,
        // Solar systems generally form flat due to the rotation that formed them, so we can think of force
        // in 2D and because of the way we structured our unity scene, the why direction is not used.
        //
        // If we imagine our currentBody at position x = 0 and z = 0 and the pulling body is at the distance
        // x = distance.x and z = x = distance.z, we can use this information to form a right triangle.
        // Now to find the angle we can take the opposite/adjacent for the tangent, which is z/x then the arctangent of
        // that value to get the angle in radians (Have to use Atan2, because atan is only quadrents 1 and 3, so this
        // will expand to cover all angles).
        double radians = Math.Atan2(distance.z, distance.x);
        
        // Now we find fx and fz which is simply the cosine for z and sin for z, multiplied by the force.
        force = new Vector3d(Math.Cos(radians) * aForce, 0, Math.Sin(radians) * aForce);

        return force;
    }


    /// <summary>
    /// This method utilizes the above forceCalc method to
    /// sum the force applied on each body by each body.
    /// </summary>
    public void updateForce()
    {
        // our list of bodies that are active in the simulation
        List<Body> bodies = Sim.Bodies.Active;
        
        // The vector "force" is the summation of the total force applied to a specific body, this is needed because
        // all bodies in the system push and pull on each other no matter how small or how large.
        Vector3d force;
        
        // The vector "aforce" is a single interaction of one body to another body, which is added to the force vector.
        Vector3d aforce = new Vector3d();
        
        // mostForce is used to determine, which body has the most pull on a specific body. This is used to help
        // determine an auto generated initial velocity, in the calcInitialVelocities() method.
        double mostForce = 0;

        // This outer foreach loop will loop to perform force calculations on each body in the system
        foreach (Body currentBody in bodies)
        {
            // Initialze the force vector for each body
            force = new Vector3d(0, 0, 0);
            
            // Now that we have a body we will loop through the bodies again to compare the
            // body to all the other bodies in the system.
            foreach (Body pullBody in bodies)
            {
                // Check to make sure we are not comparing the body to itself
                if (currentBody != pullBody)
                {
                    // use the calcForce method to get a force between the bodies
                    aforce = calcForce(currentBody, pullBody);
                    
                    // We take the magnitude of the force so that we can have a comparable
                    // double value check if it is greater than mostForce, which is initially zero
                    if (aforce.magnitude > mostForce)
                    {
                        // If true then we set mostForce equal to the aforce magnitude and then 
                        // we set the current body's MostPull attribute to the pullBody itself. This way
                        // for the current body we will know which body it is most attracted to.
                        mostForce = aforce.magnitude;
                        currentBody.MostPull = pullBody;

                    }
                    // We then add the force calculated to the total force applied to that body
                    force += aforce;
                }

            }
            
            // Now we rest the mostForce back to zero and set the current body's total force attribute
            // to the calculated total force for that body.
            mostForce = 0;
            currentBody.totalForce = force;
        }
    }

    /// <summary>
    ///  This method uses a bodies total force to calculate a new velocity
    /// This function calculates new velocity for each body.
    /// </summary>
    public void updateVelocity()
    {
        // Before calculating velocities we need to know the force applied to the bodies
        // Also we will want to know each body's mostPull attribute for the calcInitialVelocities() method.
        updateForce();
        
        // Now we want to check if any initial velocities need to be set, this will happen at the start
        // of the simulation or if new bodies are added.
        calcInitialVelocities();
        
        // List of bodies that are active in the simulation
        List<Body> bodies = Sim.Bodies.Active;
        
        // This vector is used to store the velocity for each body
        Vector3d velocity;
        
        // Initializing Acceleration 
        double xAcceleration = 0;
        double zAcceleration = 0;
        
        // Now we need to loop through all bodies to determine velocities
        foreach (Body bod in bodies)
        {
            // Velocity is always changing so we need to set the velocity to the bodies current
            // velocity so that we can manipulate it accordingly.
            velocity = bod.Vel;
            
            // We know that the standard force equation is Force = mass * acceleration, so we can manipulate
            // the equation giving acceleration = Force/ mass, which we use here to calculate the acceleration
            // of the body.
            xAcceleration = bod.totalForce.x / bod.KG;
            zAcceleration = bod.totalForce.z / bod.KG;
            
            // Now that we have calculated the acceleration, we can use the Kinematic equations to solve for a
            // new velocity, equation: Velocity final = velocity initial + acceleration * delta time.
            // We use Sim.Settings.Speed, which simulates elapsed time.
            velocity.x += xAcceleration* Sim.Settings.Speed*time;
            velocity.z += zAcceleration * Sim.Settings.Speed*time;
            
            // Since our simulation doesnt deal in the Y direction, I am just setting keeping the y velocity the sam
            // to avoid errors that may occur.
            velocity.y = bod.Vel.y;
            
            // Now we set the new velocity to the current body's velocity
            bod.Vel = velocity;
        }
    }


    /// <summary>
    /// THis function uses the new velocities to calculate a new position
    /// </summary>
    public void calcPosition()
    {
        // Before we calculate position, we need to know the velocity of the bodies
        updateVelocity();
        
        // list of all active bodies in the simulation
        List<Body> bodies = Sim.Bodies.Active;
        
        // newPos is the new position calculated for the specific body
        Vector3d newPos;
        
        // we need to loop through all bodies to set new positions for each body
        foreach (Body bod in bodies)
        {
            // We know that we are dealing with meters and velocity is meters/seconds, so in order to
            // find our new position, we need to multiply the velocity by time, which will cancel out the seconds
            // and yield a value in meters, which will then be added to the original position, creating a new
            // position
            newPos = new Vector3d(bod.Pos.x + bod.Vel.x * Sim.Settings.Speed*time, bod.Pos.y, 
                bod.Pos.z + bod.Vel.z * Sim.Settings.Speed*time);
            
            // we then set the new position to the associated body.
            bod.Pos = newPos;
        }
    }
    
    // we need a time variable so that we can scale the time with the 
    // update functions so that our system will move with real time.
    public double time;
    // Update is called once per frame
    public void Update()
    {
        // setting the time to how long it took a frame to execute
        time = Time.deltaTime;
        if (!Sim.Settings.Paused)
        {
            calcPosition();
        }
    }

}
