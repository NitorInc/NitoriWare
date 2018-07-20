using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukariCakeController : MonoBehaviour {

    // Properties
    public YukariCakeReimu Enemy;
    public List<AudioSource> AudioSources;
    public Animator YukariAnimator;
    public AudioSource YukariSource;
    public AudioClip YukariSoundSnatch, YukariCakeDimension, YukariSoundVictory, YukariSoundFail;
    public GameObject YukariFailSprites;

    // Game Variables
    public bool IsVulnerable = false;

	// Update is called once per frame
	void Update () {
        
        if(YukariAnimator.GetCurrentAnimatorStateInfo(0).IsName("YukariCakeYukariSnatch"))
        {
            if (Enemy.IsAlert && IsVulnerable)
                SetGameFailure();
        }

        if(!MicrogameController.instance.getVictoryDetermined())
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                // Here comes the snatch sequence. Better not get caught.
                YukariAnimator.SetBool("snatch", true);
            }
        }
	}

    public void SetGameVictory()
    {
        PlayVictoryAnimation();
        MicrogameController.instance.setVictory(true, true);
        Enemy.Stop();
    }

    public void SetGameFailure()
    {
        if (MicrogameController.instance.getVictoryDetermined())
            return;

        PlayFailureAnimation();
        MicrogameController.instance.setVictory(false, true);
        Enemy.Stop();
    }

    public void PlayVictoryAnimation()
    {
        YukariAnimator.Play("Victory");
    }

    public void PlayFailureAnimation()
    {
        PlayFailureSound();
		YukariAnimator.enabled = false;
        YukariFailSprites.SetActive(true);
        Enemy.PlayFailureAnimation();
    }

    public void PlayDimensionSound()
    {
        YukariSource.PlayOneShot(YukariCakeDimension);
    }

    public void PlaySnatchSound()
    {
        YukariSource.PlayOneShot(YukariSoundSnatch);
    }

    public void PlayVictorySound()
    {
        YukariSource.PlayOneShot(YukariSoundVictory);
    }

    public void PlayFailureSound()
    {
        YukariSource.PlayOneShot(YukariSoundFail);
    }
}
