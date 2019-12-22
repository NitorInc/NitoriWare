using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashMovingAudioSource : MonoBehaviour
{
    private float initialPan;
    private float speed;

    private AudioSource panSource;

    void Awake()
    {
        panSource = GetComponent<AudioSource>();
	}

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

	void Update ()
    {
        panSource.panStereo += speed * Time.deltaTime;
	}
}
