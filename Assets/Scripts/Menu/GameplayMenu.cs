using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to all menu screens that can transition to gameplay
public class GameplayMenu : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Animator animator;
#pragma warning restore 0649
    
	void Start()
    {
        animator.ResetTrigger("StartGameplay");
    }

    public void startGameplay()
    {
        GameMenu.shifting = true;
        animator.SetTrigger("StartGameplay");
    }
}
