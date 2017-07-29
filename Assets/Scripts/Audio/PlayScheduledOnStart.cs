using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScheduledOnStart : MonoBehaviour
{
    [SerializeField]
    private float playTime;
    [SerializeField]
    private int frameBuffer = 1;

    private AudioSource _audioSource;

	void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
	}

    void Update()
    {
        if (frameBuffer > 0)
        {
            frameBuffer--;
            return;
        }
        AudioHelper.playScheduled(_audioSource, playTime);
        enabled = false;
    }
}
