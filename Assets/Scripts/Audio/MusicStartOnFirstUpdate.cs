using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStartOnFirstUpdate : MonoBehaviour
{
    [SerializeField]
    private int frameBuffer = 1;

    private AudioSource _audioSource;

	void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (frameBuffer > 0)
        {
            frameBuffer--;
            return;
        }
        _audioSource.Play();
        enabled = false;
	}
}
