using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandContoller : MonoBehaviour
{
	public static RockBandContoller instance;

	public RockBandNote[] notes;

	public State state;
	public enum State
	{
		Default,
		Victory,
		Failure
	}

	void Awake()
	{
		instance = this;
	}

	public void victory()
	{
		state = State.Victory;
		MicrogameController.instance.setVictory(true, true);

		//TODO Victory animations for scene
	}

	public void failure()
	{
		state = State.Failure;
		MicrogameController.instance.setVictory(false, true);

		for (int i = 0; i < notes.Length; i++)
		{
			//TODO Failure animations for notes
			notes[i].gameObject.SetActive(false);
		}
		//TODO Failure animations for scene
	}
	
	void Update ()
	{
		if (state == State.Default && MicrogameTimer.instance.beatsLeft < 7f)
			checkForInput();
	}

	void checkForInput()
	{
		//TODO Correct arrow directions
		if (Input.GetKeyDown(KeyCode.Space))
		{
			for (int i = 0; i < notes.Length; i++)
			{
				if (notes[i].state == RockBandNote.State.InRange)
				{
					notes[i].playNote();
					if (i == notes.Length - 1)
						victory();
					return;
				}
			}
			failure();
		}
	}
}
