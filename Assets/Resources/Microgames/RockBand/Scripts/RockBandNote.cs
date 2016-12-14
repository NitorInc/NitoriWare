using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandNote : MonoBehaviour
{

	public float distancePerBeat, targetBeat;
	public bool finalNote;

	public State state;
	public enum State
	{
		Idle,
		InRange,
		Played
	}

	void Start ()
	{
		updatePosition();
	}
	
	void Update ()
	{
		updatePosition();
	}

	public void playNote()
	{
		state = State.Played;

		//TODO Play animation for notes
		gameObject.SetActive(false);
	}

	void updatePosition()
	{
		transform.position = new Vector3(distancePerBeat * (MicrogameTimer.instance.beatsLeft - targetBeat), transform.position.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Beat Zone")
		{
			state = State.InRange;
		}
		else if (other.name == "Failure Zone")
		{
			RockBandContoller.instance.failure();
		}
	}
}
