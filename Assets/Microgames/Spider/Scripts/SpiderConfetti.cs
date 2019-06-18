using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderConfetti : MonoBehaviour
{
    [SerializeField]
    private Vector2 valueRange = new Vector2(.7f, .9f);
    [SerializeField]
    private Vector2 alphaRange = new Vector2(1f, 1f);

    private ParticleSystem particles;

	void Start()
	{
        particles = GetComponent<ParticleSystem>();
	}
	
	void Update()
	{
        var main = particles.main;
        var color = new HSBColor(Random.Range(0f, 1f), 1f, MathHelper.randomRangeFromVector(valueRange)).ToColor();
        color.a = MathHelper.randomRangeFromVector(alphaRange);
        main.startColor = color;
	}
}
