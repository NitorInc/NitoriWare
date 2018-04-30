using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach to all menu screens that can transition to gameplay
public class GameplayMenu : MonoBehaviour
{
    private static bool gameplayStarting;

#pragma warning disable 0649
    [SerializeField]
    private float voiceDelay;
    [SerializeField]
    private float sceneShiftTime;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private FadingMusic bgMusicFade;
    [SerializeField]
    private AudioSource voiceSource;
#pragma warning restore 0649
    
	void Start()
    {
        gameplayStarting = false;
    }

    public void Update()
    {
        if (gameplayStarting)
        {
            startGameplay("");
        }
    }

    public void startGameplay(string scene)
    {
        animator.SetTrigger("StartGameplay");

        if (!string.IsNullOrEmpty(scene) && !gameplayStarting)
        {
            bgMusicFade.startFade();
            Invoke("playVoice", voiceDelay);
            GameController.instance.sceneShifter.startShift(scene, sceneShiftTime);
        }

        GetComponent<GameMenu>().shift((int)GameMenu.subMenu);
        gameplayStarting = true;
        enabled = false;
    }

    public void quitGame()
    {
        bgMusicFade.startFade();
        GetComponent<GameMenu>().shift((int)GameMenu.SubMenu.Quit);
        GameController.instance.sceneShifter.shiftToQuitGame(1f, .5f);
    }

    void playVoice()
    {
        voiceSource.Play();
    }

    private void OnDestroy()
    {
        gameplayStarting = false;
    }
}
