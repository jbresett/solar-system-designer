
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class SceneUtils : MonoBehaviour
    {
        public void clearScene()
        {
            List<Body> bodies = Sim.Bodies.Active;
            foreach (var body in bodies)
            {
                Bodies.deactivate(body);
            }
        }
    }
}