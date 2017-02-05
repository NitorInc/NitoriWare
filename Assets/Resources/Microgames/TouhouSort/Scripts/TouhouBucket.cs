using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouBucket : MonoBehaviour {
	// Defines a bucket of touhous
	// DO NOT add Kisume: results unpredictable

	public Transform dump;

	// List of sortable touhous
	List<Sortable> touhous;

	void Start () {
		touhous = new List<Sortable> ();

		// Add any sortables childed to this transform
		foreach (Transform child in transform)
			touhous.Add (child.GetComponent<Sortable> ());
	}

	public Sortable[] Scoop(int amount) {
		// Scoop <amount> random touhous from the bucket
		if (amount > touhous.Count) {
			amount = touhous.Count;
		}

		Sortable[] randomTouhous = new Sortable[amount];

		for (int i = 0; i < amount; i++) {
			Sortable touhou = touhous [Random.Range (0, touhous.Count)];
			randomTouhous [i] = touhou;

			touhous.Remove (touhou);
			touhou.transform.parent = dump;
		}

		return randomTouhous;
	}
}
