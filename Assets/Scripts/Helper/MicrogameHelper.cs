using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MicrogameHelper
{
  /// <summary>
  /// Returns a copied list of all microgames available in the game, with the given restriction on completeness, does not include bosses
  /// Quick call to MicrogameCollection.getMicrogames
  /// </summary>
  /// <param name="restriction"></param>
  /// <returns></returns>
  public static List<Stage.Microgame> getMicrogames(MicrogameCollection.Restriction restriction)
  {
    return GameController.instance.microgameCollection.getStageMicrogames(restriction);
  }

  /// <summary>
  /// Returns a copied list of all boss microgmaes, regardless of completion.
  /// Quick call to MicrogameCollection.getBossMicrogames
  /// </summary>
  /// <returns></returns>
  public static List<Stage.Microgame> getBossMicrogames()
  {
    return GameController.instance.microgameCollection.getStageBossMicrogames();
  }
}
