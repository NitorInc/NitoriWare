using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/TouhouSort/Traits")]
public class TouhouSortTraits : MicrogameTraits
{
	public TouhouSortSorter.Category[] categories;

    public override string GetLocalizedCommand(MicrogameSession session)
    {
        var touhouSortSession = (TouhouSortSession)session;
        return string.Format(TextHelper.getLocalizedText("microgame." + session.MicrogameId + ".command", command),
            TextHelper.getLocalizedText("microgame.TouhouSort." + touhouSortSession.category.name, touhouSortSession.category.name));

    }

	public override MicrogameSession onAccessInStage(string microgameId, int difficulty)
	{
		var category = categories[Random.Range(0, categories.Length)];
        return new TouhouSortSession(microgameId, difficulty, category);
	}
}
