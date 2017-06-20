using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameStage : Stage
{
	public static string microgameId;

	[SerializeField]
	private string forceMicrogame;

	public override Microgame getMicrogame(int num)
	{
		Microgame microgame = new Microgame(microgameId);
		microgame.microgameId = !string.IsNullOrEmpty(forceMicrogame) ? forceMicrogame : microgameId;
		return microgame;
	}

	public override int getMicrogameDifficulty(Microgame microgame, int num)
	{
		return (num % 3) + 1;
	}

	public override Interruption[] getInterruptions(int num)
	{
		if (num == 0 || num % 3 > 0)
			return new Interruption[0];

		return new Interruption[0].add(new Interruption(Interruption.SpeedChange.SpeedUp));
	}
}
