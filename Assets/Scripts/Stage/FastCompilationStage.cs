using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCompilationStage : CompilationStage
{
	[SerializeField]
	private int speed = StageController.MAX_SPEED;

	public override int getStartSpeed()
	{
		return speed;
	}

	public override Stage.Interruption[] getInterruptions(int num)
	{
		num -= roundStartIndex;
		if (num % microgamesPerRound == 0)
			return new Interruption[0].add(nextRound);
		return new Interruption[0];
	}
}
