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
    [Range(1, SpeedController.MAX_SPEED)]
    private int forceStartSpeed = 1;
    [SerializeField]
    private bool speedUpEveryCycle = false;

    public override void onStageStart(StageController stageController)
    {
        if (!string.IsNullOrEmpty(forceMicrogame) && Debug.isDebugBuild)
        {
            microgameId = forceMicrogame;
        }
        base.onStageStart(stageController);
    }

    public override StageMicrogame getMicrogame(int num)
	{
		StageMicrogame microgame = new StageMicrogame(microgameId);
		microgame.microgameId = microgameId;
		return microgame;
	}

	public override int getMicrogameDifficulty(StageMicrogame microgame, int num)
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
