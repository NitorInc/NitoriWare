using UnityEngine;

public class MilkPourState : MonoBehaviour
{
	[SerializeField]
	private bool failOnEarlyRelease;

	[SerializeField]
	private MilkPourCup cup;
	private MilkPourGameState state;

    [SerializeField]
    private Animator jugAnimator;

    [SerializeField]
    private MilkPourPourSpeedAnimation animationSpeedMult;

    [SerializeField]
    private GameObject spillParticles;

    [SerializeField]
    private AudioSource pourSource;

    [SerializeField]
    private AudioClip victoryClip;
    [SerializeField]
    private AudioClip lossClip;

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
        cup.Fill(Time.deltaTime);
        switch (state)
		{
			case MilkPourGameState.Stopped:
                if (animationSpeedMult.PourSpeedMult <= 0f)
                    OnMilkSettled();
				break;
			case MilkPourGameState.Start:
				state = Input.GetKey (KeyCode.Space) ? MilkPourGameState.Filling : MilkPourGameState.Start;
				if (state == MilkPourGameState.Filling)
                {
					OnFill ();
                    pourSource.Play();
                }
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

        pourSource.volume = animationSpeedMult.PourSpeedMult * PrefsHelper.getVolume(PrefsHelper.VolumeType.SFX);
	}

	void OnFill ()
	{
		//cup.Fill(Time.deltaTime);
        jugAnimator.SetBool("Held", true);
		if (cup.IsSpilled())
        {
			Fail();
            spillParticles.SetActive(true);
        }
	}

	void OnIdle ()
    {
        jugAnimator.SetBool("Held", false);
        state = MilkPourGameState.Stopped;
	}

    void OnMilkSettled()
    {
        if (cup.IsPassing())
            Win();
        else if (cup.IsOverfilled())
            Fail();
        else if (failOnEarlyRelease)
            Fail();
        enabled = false;
    }

	void Win ()
	{
		cup.Stop ();
		MicrogameController.instance.setVictory(true, true);
        MicrogameController.instance.playSFX(victoryClip);
		state = MilkPourGameState.Stopped;
	}

	void Fail ()
	{
		cup.Stop ();
		MicrogameController.instance.setVictory(false, true);
        MicrogameController.instance.playSFX(lossClip);
        state = MilkPourGameState.Stopped;
	}
}
