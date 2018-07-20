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
	public static List<MicrogameCollection.Microgame> getMicrogames(MicrogameTraits.Milestone restriction = MicrogameTraits.Milestone.Unfinished, bool includeBosses = false)
	{
		return MicrogameCollection.instance.microgames
            .Where(a => a.difficultyTraits[0].milestone >= restriction
            && (includeBosses || !a.difficultyTraits[0].isBossMicrogame()))
            .ToList();
	}
}
