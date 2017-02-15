using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouSortTouhouBucket : MonoBehaviour {
	// Defines a bucket of touhous
	// DO NOT add Kisume: results unpredictable

	public Transform dump;

	// List of sortable touhous
	List<TouhouSortSortable> touhous;

	void Start () {
		touhous = new List<TouhouSortSortable> ();

		// Add any sortables childed to this transform
		foreach (Transform child in transform)
			touhous.Add (child.GetComponent<TouhouSortSortable> ());
	}

	public TouhouSortSortable[] Scoop(int amount) {
		// Scoop <amount> random touhous from the bucket
		if (amount > touhous.Count) {
			amount = touhous.Count;
		}

        MouseGrabbableGroup grabGroup = dump.GetComponent<MouseGrabbableGroup>();
        TouhouSortSortable[] randomTouhous = new TouhouSortSortable[amount];

        for (int i = 0; i < amount; i++) {
			TouhouSortSortable touhou = touhous [Random.Range (0, touhous.Count)];
            MouseGrabbable grabbable = touhou.GetComponent<MouseGrabbable>();
			randomTouhous [i] = touhou;

			touhous.Remove (touhou);
			touhou.transform.parent = dump;

            grabGroup.addGrabbable(grabbable, true);
		}

		return randomTouhous;
	}
}
