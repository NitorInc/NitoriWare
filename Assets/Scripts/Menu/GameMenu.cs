using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to every menu animator
public class GameMenu : MonoBehaviour
{
    public static SubMenu subMenu = SubMenu.Splash;
    //public static SubMenu subMenu = SubMenu.Title;  //Debug purposes
    public static bool shifting;

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Animator animator;
#pragma warning restore 0649

    private SubMenu instanceSubMenu;

    public enum SubMenu
    {
        Splash = 0,
        Title = 1
    }

    void Start()
    {
        shifting = subMenu == SubMenu.Splash;
        if (instanceSubMenu != subMenu)
            updateSubMenu();
    }

    void LateUpdate()
	{
        if (instanceSubMenu != subMenu)
            updateSubMenu();
	}

    public void shift(int subMenu)
    {
        GameMenu.subMenu = (SubMenu)subMenu;
        updateSubMenu();
        shifting = true;
    }

    public void endShift()
    {
        shifting = false;
    }

    void updateSubMenu()
    {
        instanceSubMenu = subMenu;
        animator.SetInteger("SubMenu", (int)instanceSubMenu);
    }
}
