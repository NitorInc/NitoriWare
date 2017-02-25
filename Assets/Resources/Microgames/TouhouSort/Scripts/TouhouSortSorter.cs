using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouSortSorter : MonoBehaviour {
	// Primary class for TouhouSort game handling
	// Handles objects and win/loss

	// Spacing between starting sortables
	static float GAP = 2.0f;

	// Max number of sortable touhous
	public int slotCount;

	public Transform stagingArea;
	public TouhouSortTouhouBucket touhouBucket;
    public GameObject victoryDisplay;

	TouhouSortSortable[] touhous;
	Vector3[] slots;

	bool sorted;

	void Start() {
		// Scoop up as many touhous as we can
		touhous = touhouBucket.Scoop (slotCount);
		slotCount = touhous.Length;

		sorted = false;

		// Fill starting slots with touhous
		CreateSlots ();
		FillSlots ();

		// Check the sort at the start, just in case
		CheckSort ();
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

		foreach (TouhouSortSortable sortable in touhous) {
			bool thisSorted = false;

			// Get the touhou's current zone, if any
			TouhouSortDropZone currentZone = sortable.GetCurrentZone();
			
			if (currentZone) {
				thisSorted = currentZone.Belongs (sortable);
			}

			if (thisSorted != true) {
				allSorted = false;
				break;
			}
		}

		if (allSorted) {
			// Sorted
			Debug.Log("Sorted");
			sorted = true;

            victoryDisplay.SetActive(true);

			MicrogameController.instance.setVictory(true, true);
		}
		else if (sorted) {
			// Unsorted (wont ever happen)
			Debug.Log("Unsorted");
			sorted = false;

            victoryDisplay.SetActive(false);

            MicrogameController.instance.setVictory(false, true);
		}
	}
}
