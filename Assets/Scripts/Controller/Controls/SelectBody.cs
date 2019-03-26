using UnityEngine;
using UnityEngine.Internal.Experimental.UIElements;
using UnityEngine.UI;

/// <summary>
/// This class is used to display information on a celestial object
/// when the celestial object is selected via mouse click.
/// </summary>
namespace Planets
{

    public class SelectBody : MonoBehaviour
    {
        public GameObject selectMenu;
        public GameObject editMenu;
        public Body body;
        public Button selectArea;
        /// <summary> 
        /// This function initializes the class and begins listening for
        /// mouse clicks.
        /// </summary>
        private void Start()
        {
            selectArea.onClick.AddListener(select);
        }

        
        /// <summary>
        /// Updates once per frame
        ///
        /// Used unity documentation to solve this problem via
        /// https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
        /// https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Creating a ray that will shoot straight through the scene and
                //and register the first item hit
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                //If object is hit within the first 1000 unity units
                //then we will change the state of body.
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    body = hit.collider.GetComponent<Body>();
                    setSelected();
                    Debug.Log(body.name);
                }
            }
        }
        
        /// <summary>
        /// This method sets the newly selected body as selected
        /// and all other bodies to not selected.
        /// </summary>
        private void setSelected()
        {
            foreach (Body bod in Sim.Bodies.Active)
            {
                if (bod.Id == body.Id)
                {
                    bod.IsSelected = true;
                    CameraControls.changeBody(body.name);
                }
                else
                {
                    bod.IsSelected = false;
                }
            }
        }
        /// <summary> 
        /// This function looks for what celestial object was clicked and records
        /// the current data on that celestial object.
        /// </summary>
        private void select()
        {
//            RaycastHit hitInfo = new RaycastHit();
//            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
//            if (hit)
//            {
//                if (hitInfo.collider.gameObject.tag == "OrbitalBody")
//                {
//                    body = hitInfo.collider.gameObject;
//                    Text[] comps = selectMenu.GetComponentsInChildren<Text>();
//                    foreach (var v in comps)
//                    {
//                        Debug.Log(v.name);
//                    }
//                    
//                    /* Removed per old code. [TODO] SelectBody rebuild needed.
//                    comps[1].text = body.GetComponent<OrbitalBody>().Type;
//                    comps[3].text = body.GetComponent<OrbitalBody>().Name;
//                    comps[5].text = body.GetComponent<OrbitalBody>().Vel[0].ToString();
//                    comps[7].text = body.GetComponent<OrbitalBody>().Vel[1].ToString();
//                    comps[9].text = body.GetComponent<OrbitalBody>().Vel[2].ToString();
//                    */
//                }
//            }
//            
        }
    }
}