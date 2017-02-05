using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouBucket : MonoBehaviour {

	public List<Sortable> sortables;

	public Transform dump;
	
	void Start () {
		foreach (Transform child in transform)
			sortables.Add (child.GetComponent<Sortable> ());
	}

	public Sortable[] Scoop(int amount) {
		if (amount > sortables.Count) {
			amount = sortables.Count;
		}

		Sortable[] randomTouhous = new Sortable[amount];

		for (int i = 0; i < amount; i++) {
			Sortable sortable = sortables [Random.Range (0, sortables.Count)];
			randomTouhous [i] = sortable;

			sortables.Remove (sortable);
			sortable.transform.parent = dump;
		}

		return randomTouhous;
	}
}
