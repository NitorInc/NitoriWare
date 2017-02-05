using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorter : MonoBehaviour {
	// Primary class for TouhouSort game handling
	// Handles objects and win/loss

	// Spacing between starting sortables
	static float GAP = 1.0f;

	// Max number of sortable touhous
	public int slotCount;

	public Transform stagingArea;
	public TouhouBucket touhouBucket;

	Sortable[] touhous;
	Vector3[] slots;

	void Start() {
		// Scoop up as many touhous as we can
		touhous = touhouBucket.Scoop (slotCount);
		slotCount = touhous.Length;

		// Fill starting slots with touhous
		CreateSlots ();
		FillSlots ();
	}

	void CreateSlots() {
		// Instantiate a list of Vector3 objects
		// which will be the starting spots of
		// our touhous
		slots = new Vector3[slotCount];
		Vector3 origin = stagingArea.position;

		if (slotCount % 2 == 0) {
			origin.x = origin.x + (GAP / 2);
		}

		for (int i = 0; i < slotCount; i++) {
			Vector3 slot = origin;
			int multiplier = (i + 1) / 2;

			if (i % 2 == 0) {
				slot.x = origin.x + (multiplier * GAP);
			}
			else {
				slot.x = origin.x - (multiplier * GAP);
			}

			slots [i] = slot;
		}
	}

	void FillSlots() {
		// Fill instantiated slots with sortable touhous
		for (int i = 0; i < slotCount; i++) {
			touhous [i].transform.position = slots [i];
		}
	}

	public void CheckSort() {
		// Check the current state of the sort
		// End the game if everything is sorted
		bool allSorted = true;

		foreach (Sortable sortable in touhous) {
			bool sorted = false;

			// Get the touhou's current zone, if any
			DropZone currentZone = sortable.GetCurrentZone();
			
			if (currentZone) {
				int zoneCategory = (int)currentZone.category;
				sorted = currentZone.Belongs (sortable);
			}

			if (sorted != true) {
				allSorted = false;
				break;
			}
		}

		if (allSorted) {
			// Sorted
			Debug.Log("Sorted");
			MicrogameController.instance.setVictory(true, true);
		}
	}
}
