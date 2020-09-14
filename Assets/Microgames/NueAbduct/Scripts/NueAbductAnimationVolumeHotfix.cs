using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NueAbductAnimationVolumeHotfix : MonoBehaviour
{
    public float volumeMult;
    private AudioSource audioSource;

	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	void LateUpdate ()
    {
        audioSource.volume = volumeMult;
	}
}
