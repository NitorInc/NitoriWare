﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderConfetti : MonoBehaviour
{
    private ParticleSystem particles;

	void Start()
	{
        particles = GetComponent<ParticleSystem>();
	}
	
	void Update()
	{
        var main = particles.main;
        main.startColor = new HSBColor(Random.Range(0f, 1f), 1f, Random.Range(.7f, .9f)).ToColor();
	}
}
