using UnityEngine;
using System.Collections;

public class TraceShapeCursor : MonoBehaviour
{
	public int inBoundsNeeded, outOfBoundsLeft;
	public Collider2D[] essentialColliders;
	public Animator victoryAnimator;
	private AudioSource _audioSource;
	[SerializeField]
	private AudioSource resultSoundSource;
	[SerializeField]
	private AudioClip victoryClip;

	private ParticleSystem traceParticles;

	void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
		traceParticles = GetComponent<ParticleSystem>();
		Cursor.visible = false;
		_audioSource.pitch = resultSoundSource.pitch = Time.timeScale;
	}
	
	void Update ()
	{
		updateTrace();
	}

	void updateTrace()
	{
		//Enable emmissions if mouse is held down
		ParticleSystem.EmissionModule emission = traceParticles.emission;
		emission.enabled = Input.GetMouseButton(0);
		_audioSource.volume = Input.GetMouseButton(0) ? 1f : 0f;

		//When mouse is first pressed, create a particle (otherwise one won't be created before the player moves their cursor)
		if (Input.GetMouseButtonDown(0))
			traceParticles.Emit(1);

		//Snap to cursor
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);
	}

	void collide(Collider2D other)
	{
		//Player must be holding down the mouse button to hit drawing colliders
		if (!Input.GetMouseButton(0))
			return;

		other.gameObject.SetActive(false);

		//Decrement victory/loss values depending on whether the collison is in or out of bounds
		if (other.transform.parent.name == "In Bounds")
		{
			inBoundsNeeded--;
			if (inBoundsNeeded <= 0)
			{
				checkForVictory();
			}
		}
		else if (other.transform.parent.name == "Out Bounds")
		{
			outOfBoundsLeft--;
			if (outOfBoundsLeft <= 0)
			{
				failure();
			}
		}
	}

	void checkForVictory()
	{
		//The player still doesn't win unless all the "essential" colliders have been hit
		for (int i = 0; i < essentialColliders.Length; i++)
		{
			//If one is still there, no victory yet
			if (essentialColliders[i].gameObject.activeInHierarchy)
				return;
		}

		victory();
	}

	void victory()
	{
		MicrogameController.instance.setVictory(true, true);
		resultSoundSource.PlayOneShot(victoryClip);

		victoryAnimator.enabled = true;
		gameObject.SetActive(false);
	}

	void failure()
	{
		MicrogameController.instance.setVictory(false, true);
		ParticleSystem.EmissionModule emission = traceParticles.emission;
		emission.enabled = false;
		_audioSource.volume = 0f;

		//TODO Failure
		//Debug.Log("You lose!");
		enabled = false;
		disableSprite();
	}

	void disableSprite()
	{
		transform.FindChild("Sprite").gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		collide(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		collide(other);
	}

}
