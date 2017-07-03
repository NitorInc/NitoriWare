using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardCompilationStage : CompilationStage
{

	public override void onStageStart()
	{
		base.onStageStart();
		roundsCompleted = 2;
	}

	public override int getMicrogameDifficulty(Stage.Microgame microgame, int num)
	{
		return 3;
	}

	public override int getMaxLife()
	{
		return 1;
	}

	//public override Stage.Interruption[] getInterruptions(int num)
	//{
	//	num -= roundStartIndex;
	//	if (num % microgamesPerRound == 0)
	//		return new Interruption[0].add(nextRound);
	//	return new Interruption[0];
	//}
}
