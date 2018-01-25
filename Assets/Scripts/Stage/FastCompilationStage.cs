using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCompilationStage : CompilationStage
{

  [SerializeField]
  private int startSpeed;

  public override int getStartSpeed() => startSpeed;

  public override int getCustomSpeed(int microgame, Interruption interruption)
  {
    return startSpeed + getRoundSpeedOffset();
  }

  int getRoundSpeedOffset() => (roundsCompleted < 3) ? 0 : roundsCompleted - 2;

}
