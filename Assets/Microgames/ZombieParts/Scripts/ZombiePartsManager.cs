using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePartsManager : MonoBehaviour {

	[SerializeField]
	private List<ZombiePartsLimb> limbs = new List<ZombiePartsLimb>();

	// Update is called once per frame
	void Update () {
		bool isFullyComplete = true;

		for (int limbIndex = 0; limbIndex < limbs.Count; limbIndex++) {
			if (limbs[limbIndex] != null && !limbs[limbIndex].GetComplete()) {
				isFullyComplete = false;
				break;
			}
		}

		if (isFullyComplete) {
			MicrogameController.instance.setVictory(true, true);
		}
	}
}
