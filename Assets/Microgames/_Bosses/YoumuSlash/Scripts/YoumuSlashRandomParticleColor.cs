using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashRandomParticleColor : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField]
    private float particlesToEmit = 14;
    
	void Start ()
    {
        particles = GetComponent<ParticleSystem>();

        emit();
	}
	
	void emit ()
    {
        var mainModule = particles.main;
        var startColorModule = mainModule.startColor;
        var minColor = HSBColor.FromColor(startColorModule.colorMin);
        var maxColor = HSBColor.FromColor(startColorModule.colorMax);
        for (int i = 0; i < particlesToEmit; i++)
        {
            var h = Random.Range(0f, 1f);
            minColor.h = h;
            maxColor.h = h;
            startColorModule.colorMin = minColor.ToColor();
            startColorModule.colorMax = maxColor.ToColor();
            mainModule.startColor = startColorModule;
            particles.Emit(1);
        }
	}
}
