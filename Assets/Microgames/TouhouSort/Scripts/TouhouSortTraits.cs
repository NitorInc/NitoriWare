using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouSortTraits : MicrogameTraits
{
	public TouhouSortSorter.Category[] categories;
	[HideInInspector]
	public TouhouSortSorter.Category category;

	public override string localizedCommand { 
		get { 
      var format = TextHelper.getLocalizedText("microgame." + microgameId + ".command", command);
      var localizedText = TextHelper.getLocalizedText("microgame.TouhouSort." + category.name, category.name);
      return string.Format(format, localizedText);
    }
  }

	public override void onAccessInStage(string microgameId)
	{
		base.onAccessInStage(microgameId);
		category = categories[Random.Range(0, categories.Length)];
	}
}
