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
            setSubMenu((int)subMenu);
    }

    void LateUpdate()
	{
        if (instanceSubMenu != subMenu)
            setSubMenu((int)subMenu);
	}

    public void shift(int subMenu)
    {
        setSubMenu(subMenu);
        shifting = true;
    }

    public void endShift()
    {
        shifting = false;
    }

    void setSubMenu(int subMenu)
    {
        instanceSubMenu = GameMenu.subMenu = (SubMenu)subMenu;
        animator.SetInteger("SubMenu", (int)instanceSubMenu);
    }
}
