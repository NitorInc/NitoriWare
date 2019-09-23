using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to every menu animator
public class GameMenu : MonoBehaviour
{
    public static SubMenu subMenu = SubMenu.Splash;
    //public static SubMenu subMenu = SubMenu.Title;  //Debug purposes
    public static bool shifting;
    public static SubMenu shiftingFrom;

    private static GameMenu shiftOrigin;

    public enum SubMenu
    {
        Splash = 0,
        Title = 1,
        Settings = 2,
        Gamemode = 3,
        Practice = 4,
        PracticeSelect = 5,
        Credits = 6,
        Quit = 7
    }

    void Awake()
    {
        Cursor.visible = true;
        shifting = (subMenu == SubMenu.Splash);
        correctSubMenu();
        setSubMenu((int)subMenu);

        MenuAnimationUpdater updater = GetComponent<MenuAnimationUpdater>();
        if (updater != null)
            updater.updateAnimatorValues();
    }

    void correctSubMenu()
    {
        if (subMenu == SubMenu.PracticeSelect)
            subMenu = SubMenu.Practice;
    }

    public void shift(int subMenu)
    {
        if (shiftOrigin == null)
            shiftOrigin = this;
        shiftingFrom = GameMenu.subMenu;
        setSubMenu(subMenu);
        shifting = true;
    }

    public void endShift()
    {
        if (shiftOrigin != this)    //Shift cannot be ended by the same menu that starts it, this prevents early endShifts in reversible shift animations
        {
            shifting = false;
            shiftOrigin = null;
        }
    }

    void setSubMenu(int subMenu)
    {
        GameMenu.subMenu = (SubMenu)subMenu;
    }

    void setShifting(bool shifting)
    {
        GameMenu.shifting = shifting;
    }

    private void OnDestroy()
    {
        shifting = false;
    }
}
