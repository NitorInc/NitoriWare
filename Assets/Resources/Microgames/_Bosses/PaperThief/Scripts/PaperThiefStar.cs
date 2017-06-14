using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefStar : MonoBehaviour
{
	[SerializeField]
	private Vector2 velocity;
	[SerializeField]
	private float rotateTorque, cameraActivationX;

	private ParticleSystem.MainModule particleModule;
	private Rigidbody2D _rigidBody;

	void Start()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		_rigidBody.AddTorque(rotateTorque);
		transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		GetComponent<SpriteRenderer>().color = new HSBColor(Random.Range(2f / 18f, 3f / 18f), 1f, 1f).ToColor();
		particleModule = GetComponent<ParticleSystem>().main;
		particleModule.startColor = getRandomHueColor();

		if (PaperThiefNitori.instance.getState() == PaperThiefNitori.State.Gun)
		{
			particleModule.simulationSpace = ParticleSystemSimulationSpace.Custom;
			particleModule.customSimulationSpace = PaperThiefCamera.instance.transform;
		}
	}
	
	void Update()
	{
		particleModule.startColor = getRandomHueColor();

		_rigidBody.velocity = (PaperThiefCamera.instance.transform.position.x >= cameraActivationX) ? velocity : Vector2.zero;
	}

	Color getRandomHueColor()
	{
		Color color = new HSBColor(Random.Range(0f, 1f), 1f, 1f).ToColor();
		color.a = Random.Range(.25f, .75f);
		return color;
	}
}
