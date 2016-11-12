using UnityEngine;
using System.Collections;

public class TraceShapeCursor : MonoBehaviour
{
	public int inBoundsNeeded, outOfBoundsLeft;

	private ParticleSystem traceParticles;

	void Start ()
	{
		traceParticles = GetComponent<ParticleSystem>();
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

		//When mouse is first pressed, create a particle (otherwise one won't be created before the player moves their cursor)
		if (Input.GetMouseButtonDown(0))
			traceParticles.Emit(1);

		//Snap to cursor
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);
	}

	void victory()
	{
		MicrogameController.instance.setVictory(true, true);

		//TODO Victory
		Debug.Log("You win!");
	}

	void failure()
	{
		MicrogameController.instance.setVictory(false, true);

		//TODO Failure
		Debug.Log("You lose!");
		enabled = false;
	}

	void collide(Collider2D other)
	{
		if (!Input.GetMouseButton(0))
			return;

		other.enabled = false;

		if (other.transform.parent.name == "In Bounds")
		{
			inBoundsNeeded--;
			if (inBoundsNeeded <= 0)
			{
				victory();
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

	void OnTriggerEnter2D(Collider2D other)
	{
		collide(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		collide(other);
	}

}
