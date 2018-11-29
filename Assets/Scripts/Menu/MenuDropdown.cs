using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdown : MonoBehaviour
{
    public Dropdown dropdown;

#pragma warning disable 0649
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Collider2D clickCollider;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip soundClip;
#pragma warning restore 0649

    private int closedChildCount;
    private MenuDropdown[] neighbors;
    private bool wasOpenLastFrame;
    private int startFrame;

    void Start()
	{
        closedChildCount = transform.childCount;
        neighbors = transform.parent.GetComponentsInChildren<MenuDropdown>();
        wasOpenLastFrame = false;
        startFrame = Time.frameCount;
	}
	
	void Update()
    {
        if (GameMenu.shifting)
        {
            if (isOpen())
            {
                animator.SetBool("CanHighlight", false);
                dropdown.Hide();
            }
            return;
        }

        animator.SetBool("CanHighlight", CameraHelper.isMouseOver(clickCollider) && !areAnyNeighborsOpen(true));
        
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
        if (Time.frameCount > startFrame)
        {
            animator.Play("UpdateValue");
            sfxSource.pitch = 1f;
            sfxSource.PlayOneShot(soundClip);
        }
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
