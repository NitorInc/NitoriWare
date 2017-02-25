using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouSortDropZone : MonoBehaviour {
	// A zone that a sortable must be sorted into

	// The category that this zone represents
	public TouhouSortSortable.Style style;
	// Inverts the zone (e.g. no hats allowed)
	public bool invert;
    
    public void SetCategory(TouhouSortSortable.Style style, bool invert, Sprite icon) {

        this.style = style;
        this.invert = invert;

        this.GetComponent<SpriteRenderer>().sprite = icon;
    }

	public bool Belongs(TouhouSortSortable sortable) {
		// Checks if a given sortable belongs in this zone
		bool belongs = false;

		int zoneTypeId = (int)style;
		foreach (TouhouSortSortable.Style style in sortable.styles) {
			int typeId = (int)style;

			if (typeId == zoneTypeId) {
				belongs = true;
				break;
			}
		}

		if (invert) {
			belongs = !belongs;
		}

		return belongs;
	}
}
