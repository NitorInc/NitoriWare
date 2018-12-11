using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceAutoPan : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float xPosBoundMult = 1f;
    
	void Start ()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
	}
	
	void Update ()
    {
        if (audioSource.isPlaying)
            audioSource.panStereo = AudioHelper.getAudioPan(transform.position.x, xPosBoundMult);
	}
}
