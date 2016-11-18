﻿using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour
{

	public SpiderFood food;

	public float maxMouthAngle, mouthStartDistance, eatRadius, munchSpeed;
	public Transform top, bottom, attachTo, target;

	private float mouthAngle;
	private bool mouthClosing;

	public AudioSource feedSound;

	void Awake ()
	{
		mouthAngle = 0f;
		reset();
	}

	public void reset()
	{
		mouthAngle = 0f;
		transform.rotation = Quaternion.identity;
		updateMouth();
		food.eaten = food.grabbed = false;
		transform.localScale = Vector3.one;
		food.GetComponent<ParticleSystem>().SetParticles(new ParticleSystem.Particle[0], 0);

		//food.GetComponent<ParticleSystem>().emissionRate = 0f;


		int tries = 100;

		do
		{
			transform.parent.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0f);
			tries--;
		}
		while (isInCenter(transform.position) && tries > 0);


		if (tries <= 0)
			Debug.Log("IT SIMPLY CANNOT BE DONE");

		do
		{
			tries--;
			food.transform.position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), food.transform.position.z);


			//Debug.Log(((Vector2)transform.position - (Vector2)food.transform.position).magnitude);
		}
		while ((isInCenter(food.transform.position) || ((Vector2)transform.position - (Vector2)food.transform.position).magnitude < 4f)
			&& tries > 0);



		if (tries <= 0)
			Debug.Log("IT SIMPLY CANNOT BE DONE");

	}

	bool isInCenter(Vector3 position)
	{
		return Mathf.Abs(position.x) < 2f  && Mathf.Abs(position.y) < 1.5f;
	}

	void LateUpdate ()
	{
		transform.position = attachTo.transform.position;

		if (food.eaten)
			munch();
		else if (food.grabbed)
			lookAtCursor();

	}

	void munch()
	{
		if (mouthClosing)
		{
			mouthAngle -= Time.deltaTime * munchSpeed;
			mouthClosing = mouthAngle >= 0f;
			mouthAngle = Mathf.Max(0f, mouthAngle);
		}
		else
		{
			mouthAngle += Time.deltaTime * munchSpeed;
			mouthClosing = mouthAngle > maxMouthAngle / 3f;
			mouthAngle = Mathf.Min(maxMouthAngle / 3f, mouthAngle);
		}
		updateMouth();
	}

	void lookAtCursor()
	{

		Vector3 cursorPosition = CameraHelper.getCursorPosition();

		float angle = MathHelper.getVectorAngle2D((Vector2)(transform.position - cursorPosition)) * Mathf.Deg2Rad,
			distance = ((Vector2)(transform.position - cursorPosition)).magnitude;

		transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);

		distance /= transform.localScale.x / 2f;

		mouthAngle = Mathf.Lerp(maxMouthAngle, 0f, distance / mouthStartDistance);
		updateMouth();

		bool flipped = cursorPosition.x > transform.position.x;
        transform.localScale = new Vector3(1f, flipped ? -1f : 1f, 1f);
		

		if (distance <= eatRadius)
		{
			food.eaten = true;
			mouthClosing = true;

			ParticleHelper.setEmissionRate(food.GetComponent<ParticleSystem>(), food.particleRate);

			//food.GetComponent<ParticleSystem>().emission.rate = new ParticleSystem.MinMaxCurve(food.particleRate);

			MicrogameController.instance.setVictory(true, true);
			
			//feedSound.pitch = 1.1f * Time.timeScale;
			//feedSound.Play();

			//CameraController.instance.setShakeCoolRate(0f);
			//CameraController.instance.setScreenShake(1f);
		}
	}

	void updateMouth()
	{
		top.localRotation = Quaternion.Euler(0f, 0f, -mouthAngle * Mathf.Rad2Deg);
		bottom.localRotation = Quaternion.Euler(0f, 0f, mouthAngle * Mathf.Rad2Deg);
	}
}
