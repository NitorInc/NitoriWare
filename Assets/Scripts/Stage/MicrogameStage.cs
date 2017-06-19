using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameStage : Stage
{
	public static string microgameId;

	[SerializeField]
	private string forceMicrogame;

	void Start()
	{
		
	}

	public override Microgame getMicrogame(int num)
	{
		Microgame microgame = new Microgame();
		microgame.microgameId = string.IsNullOrEmpty(forceMicrogame) ? forceMicrogame : microgameId;
		microgame.baseDifficulty = (num % 3) + 1;
		return microgame;
	}

	public override int getMicrogameDifficulty(Microgame microgame)
	{
		return microgame.baseDifficulty;
	}

	public override Interruption[] getInterruptions(int num)
	{
		if (num == 0 || num % 3 > 0)
			return new Interruption[0];

		Interruption silentSpeedUp = new Interruption();
		silentSpeedUp.speedChange = Interruption.SpeedChange.SpeedUp;
		return new Interruption[0].add(silentSpeedUp);
	}
}
