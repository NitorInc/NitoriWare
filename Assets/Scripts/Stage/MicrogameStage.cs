using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameStage : Stage
{
	public static string microgameId;

    [Header("Override settings for debugging")]
    [Header("Force Microgame must be changed when played from this scene")]
    [Header("DO NOT COMMIT CHANGES TO THESE!")]
	[SerializeField]
	private string forceMicrogame;
    [SerializeField]
    private int forceDifficulty;
    [SerializeField]
    [Range(1, StageController.MAX_SPEED)]
    private int forceStartSpeed = 1;
    [SerializeField]
    private bool speedUpEveryCycle = false;

    public override void onStageStart()
    {
        //Update collection if microgame is forced, in case it's in the project but hasn't been added to the collection, for debugging
        if (!string.IsNullOrEmpty(forceMicrogame))
        {
            MicrogameCollection.instance.updateMicrogames();
            microgameId = forceMicrogame;
        }
        base.onStageStart();
    }

    public override Microgame getMicrogame(int num)
	{
		Microgame microgame = new Microgame(microgameId);
		microgame.microgameId = microgameId;
		return microgame;
	}

	public override int getMicrogameDifficulty(Microgame microgame, int num)
	{
		return forceDifficulty < 1 ? ((num % 3) + 1) : forceDifficulty;
	}

    public override int getStartSpeed()
    {
        return forceStartSpeed;
    }

    public override string getDiscordState(int microgameIndex)
    {
        return TextHelper.getLocalizedText("microgame." + microgameId + ".igname", microgameId);
    }

    public override Interruption[] getInterruptions(int num)
	{
		if ((!speedUpEveryCycle) && (num == 0 || num % 3 > 0))
			return new Interruption[0];

		return new Interruption[0].add(new Interruption(Interruption.SpeedChange.SpeedUp));
	}
}
