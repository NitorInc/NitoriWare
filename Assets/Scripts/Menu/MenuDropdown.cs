using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdown : MonoBehaviour
{
    public Dropdown dropdown;

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Collider2D clickCollider;
#pragma warning restore 0649

    private int closedChildCount;
    private bool hiding;
    private MenuDropdown[] neighbors;
    private bool wasOpenLastFrame;

	void Start()
	{
        closedChildCount = transform.childCount;
        neighbors = transform.parent.GetComponentsInChildren<MenuDropdown>();
        wasOpenLastFrame = false;
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
        
        //Gotta force these sometimes
        if (isOpen() && Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
        {
            dropdown.Hide();
        }
        else if (!areAnyNeighborsOpen(true) && Input.GetMouseButtonUp(0) && CameraHelper.isMouseOver(clickCollider))
        {
            dropdown.Show();
        }

        if (isOpen() && !wasOpenLastFrame)
        {
            hideNeighbors(false);
        }

        wasOpenLastFrame = isOpen();

    }

    public void press(int value)
    {
        animator.Play("UpdateValue");
    }

    bool areAnyNeighborsOpen(bool includeSelf)
    {
        foreach (var neighbor in neighbors)
        {
            if (neighbor.isOpen() && (includeSelf || neighbor != this))
                return true;
        }
        return false;
    }

    void hideNeighbors(bool includeSelf)
    {
        foreach (var neighbor in neighbors)
        {
            if (includeSelf || neighbor != this)
                neighbor.dropdown.Hide();
        }
    }

    public bool isOpen()
    {
        return transform.childCount > closedChildCount;
    }

    
}
