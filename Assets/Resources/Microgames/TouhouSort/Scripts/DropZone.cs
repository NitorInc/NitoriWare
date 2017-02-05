using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour {
	// A zone that a sortable must be sorted into

	// The category that this zone represents
	public Sortable.Style category;
	// Inverts the zone (e.g. no hats allowed)
	public bool invert;

	public Sorter sorter;
	public SpriteRenderer spriteRenderer;

	public bool Belongs(Sortable sortable) {
		// Checks if a given sortable belongs in this zone
		bool belongs = false;

		int zoneTypeId = (int)category;
		foreach (Sortable.Style style in sortable.styles) {
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
		Sortable sortable = other.GetComponentInParent<Sortable>();
		sortable.EnterZone(gameObject.GetComponent<DropZone>());

		sorter.SendMessage ("CheckSort");
	}

	void OnTriggerExit2D(Collider2D other) {
		Sortable sortable = other.GetComponentInParent<Sortable>();
		sortable.ExitZone(gameObject.GetComponent<DropZone>());

		sorter.SendMessage ("CheckSort");
	}
}
