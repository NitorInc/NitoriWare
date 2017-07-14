using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdown : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Collider2D clickCollider;
#pragma warning restore 0649

    private int closedChildCount;
    private bool hiding;

	void Start()
	{
        closedChildCount = transform.childCount;
	}
	
	void Update()
    {
        if (GameMenu.shifting)
        {
            if (!isOpen())
            {
                animator.SetBool("CanHighlight", false);
                dropdown.Hide();
                hiding = true;
            }
            return;
        }
        else
            hiding = false;

        animator.SetBool("CanHighlight", CameraHelper.isMouseOver(clickCollider) && !isOpen());
        
    }

    public void setValue(int value)
    {

    }

    bool isOpen()
    {
        return transform.childCount > closedChildCount;
    }
}
