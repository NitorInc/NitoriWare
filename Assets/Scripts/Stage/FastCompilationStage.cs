using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/Fast Compilation Stage")]
public class FastCompilationStage : CompilationStage
{
	[SerializeField]
	private int startSpeed;

	public override int getStartSpeed()
	{
		return startSpeed;
	}

    //public override int getCustomSpeed(int microgame, Interruption interruption)
    //{
    //	return startSpeed + getRoundSpeedOffset();
    //}

    public override int GetRoundSpeed(int microgame)
    {
		return (roundsCompleted < 3) ? 0 : roundsCompleted - 2;
	}
}
