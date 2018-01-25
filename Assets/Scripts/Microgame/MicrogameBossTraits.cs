using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameBossTraits : MicrogameTraits
{

#pragma warning disable 0649
  [SerializeField]
  private float _victoryEndBeats, _failureEndBeats;
#pragma warning disable 0649

  public override float getDurationInBeats() => float.PositiveInfinity;

  public float victoryEndBeats => _victoryEndBeats;
  public float failureEndBeats => _failureEndBeats;

}