using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandContoller : MonoBehaviour
{
	public static RockBandContoller instance;

	public RockBandNote[] notes;
	public Animator kyoani, mystiaAnimator;
	public RockBandLight[] lights;
	public AudioClip victoryClip, failureClip;
	public AudioClip[] noteHitClips;

	private AudioSource _audioSource;
	private State state;
	public enum State
	{
		Default,
		Victory,
		Failure,
		Hit
	}

	void Awake()
	{
		instance = this;
		_audioSource = GetComponent<AudioSource>();
		_audioSource.pitch = Time.timeScale;
	}

	public void victory()
	{
		setState(State.Victory);
		MicrogameController.instance.setVictory(true, true);
		foreach (RockBandLight light in lights)
		{
			light.onVictory();
		}
		_audioSource.PlayOneShot(victoryClip);
	}

	public void failure()
	{
		setState(State.Failure);
		MicrogameController.instance.setVictory(false, true);

		for (int i = 0; i < notes.Length; i++)
		{
			notes[i].gameObject.SetActive(false);
		}
		foreach (RockBandLight light in lights)
		{
			Animator lightAnimator = light.GetComponentInChildren<Animator>();
			if (lightAnimator != null)
				lightAnimator.enabled = false;
		}
		_audioSource.PlayOneShot(failureClip);
	}

	void hitNote()
	{
		setState(State.Hit);
		foreach (RockBandLight light in lights)
		{
			light.onHit();
		}
		_audioSource.PlayOneShot(noteHitClips[Random.Range(0, noteHitClips.Length)]);
	}
	
	void Update ()
	{
		if (state == State.Hit)
			setState(State.Default);

		if (state == State.Default && MicrogameTimer.instance.beatsLeft < 7f)
			checkForInput();

	}

	void checkForInput()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow)
			|| Input.GetKeyDown(KeyCode.UpArrow)
			|| Input.GetKeyDown(KeyCode.LeftArrow)
			|| Input.GetKeyDown(KeyCode.DownArrow))
		{
			for (int i = 0; i < notes.Length; i++)
			{
				if (notes[i].state == RockBandNote.State.InRange)
				{
					if (Input.GetKeyDown(notes[i].key))
					{
						notes[i].playNote();
						if (i == notes.Length - 1)
							victory();
						else
						{
							hitNote();
						}
					}
					else
					{
						failure();
					}
					return;
				}
			}
			failure();
		}
	}

	void setState(State state)
	{
		this.state = state;
		kyoani.SetInteger("state", (int)state);
		mystiaAnimator.SetInteger("state", (int)state);

	}
}
