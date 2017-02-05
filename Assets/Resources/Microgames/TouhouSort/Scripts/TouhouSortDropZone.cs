using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouSortDropZone : MonoBehaviour {
	// A zone that a sortable must be sorted into

	// The category that this zone represents
	public TouhouSortSortable.Style category;
	// Inverts the zone (e.g. no hats allowed)
	public bool invert;

	public TouhouSortSorter sorter;

	public bool Belongs(TouhouSortSortable sortable) {
		// Checks if a given sortable belongs in this zone
		bool belongs = false;

		int zoneTypeId = (int)category;
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

	void OnTriggerEnter2D(Collider2D other) {
		TouhouSortSortable sortable = other.GetComponentInParent<TouhouSortSortable>();
		sortable.EnterZone(gameObject.GetComponent<TouhouSortDropZone>());

		sorter.SendMessage ("CheckSort");
	}

	void OnTriggerExit2D(Collider2D other) {
		TouhouSortSortable sortable = other.GetComponentInParent<TouhouSortSortable>();
		sortable.ExitZone(gameObject.GetComponent<TouhouSortDropZone>());

		sorter.SendMessage ("CheckSort");
	}
}
