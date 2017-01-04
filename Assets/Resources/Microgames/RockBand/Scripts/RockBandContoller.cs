using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandContoller : MonoBehaviour
{
	public static RockBandContoller instance;

	public RockBandNote[] notes;
	public Animator kyoani;

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
	}

	public void victory()
	{
		setState(State.Victory);
		MicrogameController.instance.setVictory(true, true);
	}

	public void failure()
	{
		setState(State.Failure);
		MicrogameController.instance.setVictory(false, true);

		for (int i = 0; i < notes.Length; i++)
		{
			notes[i].gameObject.SetActive(false);
		}
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
							setState(State.Hit);
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
	}
}
