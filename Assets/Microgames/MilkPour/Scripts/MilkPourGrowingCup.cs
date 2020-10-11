using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPourGrowingCup : MilkPourCup {
	
	// The minimum fill necessary for the cup to start growing.
	[SerializeField]
	private float cupGrowthInitThreshold;
	
	// The amount of space that the cup and fill lines grow per second, once it starts growing.
	[SerializeField]
	private float cupGrowthPerSecond;
    [SerializeField]
    private Transform charTransform;
    [SerializeField]
    private float charUpSpeed = 1f;

    public override void Fill(float deltaTime)
	{
		base.Fill(deltaTime);
		
		if (IsSpilled())
			Stop();

		if (FillHeight >= cupGrowthInitThreshold && !Stopped)
		{
			var growth = cupGrowthPerSecond * deltaTime * animationSpeedMult.PourSpeedMult;
			GlassHeight += growth;
			LowerFillLineHeight += growth;
			UpperFillLineHeight += growth;
            charTransform.position += Vector3.up * (growth * charUpSpeed / cupGrowthPerSecond);
		}
	}
}
