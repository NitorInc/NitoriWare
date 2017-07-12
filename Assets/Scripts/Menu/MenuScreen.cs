using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    //public static SubMenu gameSubMenu = SubMenu.Splash;
    public static SubMenu gameSubMenu = SubMenu.Title;  //Debug purposes

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Animator animator;
#pragma warning restore 0649

    private SubMenu subMenu;

    public enum SubMenu
    {
        Splash = 0,
        Title = 1
    }

    void Start()
    {
        if (subMenu != gameSubMenu)
            updateSubMenu();
    }

    void LateUpdate()
	{
        if (subMenu != gameSubMenu)
            updateSubMenu();
	}

    void updateSubMenu()
    {
        subMenu = gameSubMenu;
        animator.SetInteger("SubMenu", (int)subMenu);
    }
}
