using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class MicrogameHelper
{
    /// <summary>
    /// Returns a list of all microgames with milestone equal to or greater than the restriction
    /// Quick call to MicrogameCollection.getMicrogames
    /// </summary>
    /// <param name="restriction"></param>
    /// <returns></returns>
    public static List<Microgame> getMicrogames(Microgame.Milestone restriction = Microgame.Milestone.Unfinished, bool includeBosses = false)
	{
		return MicrogameCollection.LoadAllMicrogames()
            .Where(a => a.milestone >= restriction
            && (includeBosses || !a.isBossMicrogame()))
            .ToList();
	}
}
