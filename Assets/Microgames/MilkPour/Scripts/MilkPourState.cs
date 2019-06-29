using UnityEngine;

public class MilkPourState : MonoBehaviour
{
	[SerializeField]
	private bool failOnEarlyRelease;

	[SerializeField]
	private MilkPourCup cup;
	private MilkPourGameState state;

	private enum MilkPourGameState
	{
		Start,
		Filling,
		Idle,
		Stopped
	}
        
	void Start ()
	{
		state = MilkPourGameState.Start;
	}

	void Update ()
	{
		switch (state)
		{
			case MilkPourGameState.Stopped:
				break;
			case MilkPourGameState.Start:
				state = Input.GetKey (KeyCode.Space) ? MilkPourGameState.Filling : MilkPourGameState.Start;
				if (state == MilkPourGameState.Filling)
					OnFill ();
				break;
			case MilkPourGameState.Filling:
			case MilkPourGameState.Idle:
				state = Input.GetKey (KeyCode.Space) ? MilkPourGameState.Filling : MilkPourGameState.Idle;
				if (state == MilkPourGameState.Filling)
					OnFill ();
				else
					OnIdle ();
				break;
		}
	}

	void OnFill ()
	{
		cup.Fill(Time.deltaTime);
		if (cup.IsSpilled())
			Fail();
	}

	void OnIdle ()
	{
		if (cup.IsPassing())
			Win();
		else if (cup.IsOverfilled ())
			Fail ();
		else if (failOnEarlyRelease)
			Fail ();
	}

	void Win ()
	{
		cup.Stop ();
		MicrogameController.instance.setVictory(true, true);
		state = MilkPourGameState.Stopped;
	}

	void Fail ()
	{
		cup.Stop ();
		MicrogameController.instance.setVictory(false, true);
		state = MilkPourGameState.Stopped;
	}
}
