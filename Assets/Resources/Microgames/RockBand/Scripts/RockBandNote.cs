using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandNote : MonoBehaviour
{

	public float distancePerBeat, targetBeat;
	public KeyCode key;
	public KeyCode[] possibleKeys;

	[SerializeField]
	private Animator noteAnimator;
	private Animator beatAnimator;

	public State state;
	public enum State
	{
		Idle,
		InRange,
		Played
	}

	void Start()
	{
		int keyIndex = Random.Range(0, possibleKeys.Length);
		key = possibleKeys[keyIndex];
		transform.rotation = Quaternion.Euler(0f, 0f, 90f * keyIndex);

		updatePosition();
	}
	
	void Update()
	{
		if (state != State.Played)
			updatePosition();
		else
			transform.moveTowards(beatAnimator.transform.position, 15f);
	}

	public void playNote()
	{
		state = State.Played;
		noteAnimator.Play("Hit");
		GetComponentInChildren<ParticleSystem>().Play();
		beatAnimator.Rebind();
		beatAnimator.Play("Hit");
	}

	void updatePosition()
	{
		transform.localPosition = new Vector3(distancePerBeat * (MicrogameTimer.instance.beatsLeft - targetBeat),
			transform.localPosition.y, transform.localPosition.z);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "RockBandBeatZone")
		{
			state = State.InRange;
			if (beatAnimator == null)
				beatAnimator = other.GetComponent<Animator>();
		}
		else if (other.name == "Failure Zone")
		{
			RockBandContoller.instance.failure();
		}
	}
}
