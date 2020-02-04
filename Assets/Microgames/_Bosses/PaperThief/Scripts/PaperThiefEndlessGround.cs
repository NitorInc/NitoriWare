using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefEndlessGround : MonoBehaviour
{
	[SerializeField]
	private Transform[] shiftTransforms;
    [SerializeField]
    private ParticleSystem[] shiftParticles;

	void Update()
	{
		if (MainCameraSingleton.instance.transform.parent.parent == null)
			return;
		while (transform.position.x < MainCameraSingleton.instance.transform.position.x && CameraHelper.isObjectOffscreen(transform, transform.localScale.x / 2f))
		{
            for (int i = 0; i < shiftTransforms.Length; i++)
            {
                shiftTransforms[i].localPosition += Vector3.left * transform.localScale.x;
            }
            for (int i = 0; i < shiftParticles.Length; i++)
            {
                var main = shiftParticles[i].main;
                ParticleSystem.Particle[] allParticles = new ParticleSystem.Particle[main.maxParticles];
                int activeParticleCount = shiftParticles[i].GetParticles(allParticles);
                for (int j = 0; j < activeParticleCount; j++)
                {
                    allParticles[j].position += Vector3.left * transform.localScale.x;
                }
                shiftParticles[i].SetParticles(allParticles, activeParticleCount);
            }
		}
	}
}
