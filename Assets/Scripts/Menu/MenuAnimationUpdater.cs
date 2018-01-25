using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimationUpdater : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private bool updateSubMenu = true, updateShifting;
#pragma warning restore 0649

    private GameMenu.SubMenu instanceSubMenu;
    private bool instanceShifting;

    void Start()
	{
        updateAnimatorValues();
	}

    void LateUpdate()
    {
        updateAnimatorValues();
    }

    public void updateAnimatorValues()
    {
        if (updateSubMenu && instanceSubMenu != GameMenu.subMenu)
        {
            animator.SetInteger("SubMenu", (int)GameMenu.subMenu);
            instanceSubMenu = GameMenu.subMenu;
        }
        if (updateShifting && instanceShifting != GameMenu.shifting)
        {
            animator.SetBool("Shifting", GameMenu.shifting);
            instanceShifting = GameMenu.shifting;
        }
    }
}
