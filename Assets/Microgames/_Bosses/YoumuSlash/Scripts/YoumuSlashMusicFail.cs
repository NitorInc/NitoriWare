using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashMusicFail : MonoBehaviour
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
        animator.enabled = true;
	}

    private void LateUpdate()
    {
        if (animator.enabled)
            musicSource.pitch = Time.timeScale * pitchMult;
    }
}
