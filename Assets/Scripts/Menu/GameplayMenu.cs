using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to all menu screens that can transition to gameplay
public class GameplayMenu : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private float voiceDelay;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private FadingMusic bgMusicFade;
    [SerializeField]
    private AudioSource voiceSource;
#pragma warning restore 0649
    
	void Start()
    {
        animator.ResetTrigger("StartGameplay");
    }

    public void startGameplay()
    {
        GetComponent<GameMenu>().shift((int)GameMenu.subMenu);
        animator.SetTrigger("StartGameplay");
        bgMusicFade.startFade();
        Invoke("playVoice", voiceDelay);
    }

    void playVoice()
    {
        voiceSource.Play();
    }
}
