using System.Collections.Generic;
using UnityEngine;

namespace Controller.UI
{
    public class CloseUIs : MonoBehaviour
    {
        public GameObject[] menuList;

        private void OnEnable()
        {
            foreach (var menu in menuList)
            {
                if(menu!=this.gameObject)
                    menu.SetActive(false);
            }
        }


        public void setMenus(GameObject[] menus)
        {
            menuList = menus;
        }

    }
    
}