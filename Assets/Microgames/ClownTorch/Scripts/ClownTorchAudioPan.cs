using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownTorchAudioPan : MonoBehaviour
{

    private AudioSource audioSource;
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update ()
    {
        audioSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
	}
}
