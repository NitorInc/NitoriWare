using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCompilationStage : CompilationStage
{
	[SerializeField]
	private int startSpeed;

	public override int getStartSpeed()
	{
		return startSpeed;
	}

	public override int getCustomSpeed(int microgame, Interruption interruption)
	{
		return startSpeed + getRoundSpeedOffset();
	}

	int getRoundSpeedOffset()
	{
		return (roundsCompleted < 3) ? 0 : roundsCompleted - 2;
	}

	//public override Stage.Interruption[] getInterruptions(int num)
	//{
	//	num -= roundStartIndex;
	//	if (num % microgamesPerRound == 0)
	//		return new Interruption[0].add(nextRound);
	//	return new Interruption[0];
	//}
}
