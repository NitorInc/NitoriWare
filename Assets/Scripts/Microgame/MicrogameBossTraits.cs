using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameBossTraits : MicrogameTraits
{

#pragma warning disable 0649
    [SerializeField]
	private float _victoryEndBeats, _failureEndBeats;
#pragma warning disable 0649

    public override float getDurationInBeats()
	{
		return float.PositiveInfinity;
	}

	public float victoryEndBeats{ get { return _victoryEndBeats; } set { } }
	public float failureEndBeats { get { return _failureEndBeats; } set { } }
}