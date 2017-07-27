using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayAfterDelay : MonoBehaviour
{
    public ParticleSystem particles;
    public float timer;
    
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            particles.Play();
            timer = 0f;
            enabled = false;
        }

	}
}
