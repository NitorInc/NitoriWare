using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySFXOnStart : MonoBehaviour {

    public AudioClip clip;
	void Start () {
        MicrogameController.instance.playSFX(clip, MicrogameController.instance.getSFXSource().panStereo);
	}
}
