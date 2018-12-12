using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashMusicFail : MonoBehaviour
{
    Animator animator;
    
	void Start ()
    {
        animator = GetComponent<Animator>();
        YoumuSlashPlayerController.onFail += onFail;
	}
	
	void onFail()
    {
        animator.enabled = true;
	}
}
