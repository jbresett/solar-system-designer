using System.Linq;
using Boo.Lang;
using UnityEngine;

namespace Controller.UI
{
    public class InitCloseComps : MonoBehaviour
    {
        private string[] menuTags = {"UIMenu"};
        private List<GameObject> menus = new List<GameObject>();
        private void Start()
        {
            GameObject[] gameObjects = Resources.FindObjectsOfTypeAll<GameObject>() ;
            foreach (var go in gameObjects)
            {
                if (menuTags.Contains(go.tag))
                {
                    CloseUIs close = go.AddComponent(typeof(CloseUIs))as CloseUIs;
                    menus.Add(go);
                }
            }

            foreach (var menu in menus)
            {
                menu.GetComponent<CloseUIs>().setMenus(menus.ToArray());
            }
        }
    }
}