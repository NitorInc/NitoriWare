using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashMusicEffectManager : MonoBehaviour
{
    [SerializeField]
    private float pitchMult = 1f;

    Animator animator;
    AudioSource musicSource;
    
	void Start ()
    {
        animator = GetComponent<Animator>();
        YoumuSlashPlayerController.onFail += onFail;
        musicSource = GetComponent<AudioSource>();
	}
	
	void onFail()
    {
        animator.SetTrigger("Fail");
	}

    private void LateUpdate()
    {
        if (MicrogameController.instance.getVictoryDetermined() && !MicrogameController.instance.getVictory())
            musicSource.pitch = Time.timeScale * pitchMult;
    }
}
