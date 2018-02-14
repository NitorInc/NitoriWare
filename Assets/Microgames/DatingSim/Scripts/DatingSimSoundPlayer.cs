using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimSoundPlayer : MonoBehaviour
{
    public AudioClip winClip, lossClip;
    
	void Start ()
    {
        DatingSimOptionController.OnWinning += PlayWinSound;
        DatingSimOptionController.OnLosing += PlayLossSound;
    }
	
    void PlayWinSound()
    {
        MicrogameController.instance.playSFX(winClip);
    }

    void PlayLossSound()
    {
        MicrogameController.instance.playSFX(lossClip);
    }
}
