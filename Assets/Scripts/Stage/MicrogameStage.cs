using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/Microgame Stage")]
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
    [Range(1, SpeedController.MaxSpeed)]
    private int forceStartSpeed = 1;
    [SerializeField]
    private bool speedUpEveryCycle = false;


    public override void InitStage(int seed = 0)
    {
        base.InitStage(seed);
        if (!string.IsNullOrEmpty(forceMicrogame) && Debug.isDebugBuild)
        {
            microgameId = forceMicrogame;
        }
    }

    public override StageMicrogame getMicrogame(int num)
	{
		StageMicrogame microgame = new StageMicrogame(MicrogameCollection.LoadMicrogame(microgameId));
		return microgame;
	}

    public override int getStartSpeed()
    {
        return forceStartSpeed;
    }

    public override string getDiscordState(int microgameIndex)
    {
        return TextHelper.getLocalizedText("microgame." + microgameId + ".igname", microgameId);
    }

    public override int GetRound(int microgameIndex) => 0;

    //   public override Interruption[] getInterruptions(int num)
    //{
    //	if ((!speedUpEveryCycle) && (num == 0 || num % 3 > 0))
    //		return new Interruption[0];

    //	return new Interruption[0].add(new Interruption(Interruption.SpeedChange.SpeedUp));
    //}
}
