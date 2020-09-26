using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefShot : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
	private float shrinkSpeed;
	[SerializeField]
	private int explosionSmokeCount;
	[SerializeField]
	private ParticleSystem trailParticles, explosionParticles;
#pragma warning restore 0649

    private Rigidbody2D _rigidBody;
	private ParticleSystem.MainModule trailParticlesModule, explosionParticlesModule;
	private bool dead, shrank;

	void Start()
	{
		dead = false;
		trailParticlesModule = trailParticles.main;
		_rigidBody = GetComponent<Rigidbody2D>();

		trailParticlesModule = trailParticles.main;
		trailParticlesModule.simulationSpace = ParticleSystemSimulationSpace.Custom;
		trailParticlesModule.customSimulationSpace = PaperThiefCamera.instance.transform;
		explosionParticlesModule = explosionParticles.main;
		explosionParticlesModule.simulationSpace = ParticleSystemSimulationSpace.Custom;
		explosionParticlesModule.customSimulationSpace = PaperThiefCamera.instance.transform;

	}
	
	void LateUpdate ()
	{
		if (PaperThiefNitori.dead)
		{
			_rigidBody.velocity = Vector2.zero;
			_rigidBody.freezeRotation = true;
			trailParticlesModule.simulationSpeed = explosionParticlesModule.simulationSpeed = 0f;
			enabled = false;
		}
		else if (dead && !shrank)
		{
			transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;
			if (transform.localScale.x <= 0f)
			{
				transform.localScale = Vector3.zero;
				Invoke("destroy", 1f);
				shrank = true;
			}
		}
	}

	void destroy()
	{
		if (!PaperThiefNitori.dead)
			Destroy(gameObject);
	}

	public void kill()
	{
		//_rigidBody.velocity = PaperThiefNitori.instance.getRigidBody().velocity;
		explosionParticles.Emit(explosionSmokeCount);
		GetComponent<Collider2D>().enabled = false;
        var trailEmission = trailParticles.emission;
        var trailCurve = trailEmission.rateOverTime;
        trailCurve.constant = 0f;
        trailEmission.rateOverTime = trailCurve;
        
		dead = true;
	}

}
