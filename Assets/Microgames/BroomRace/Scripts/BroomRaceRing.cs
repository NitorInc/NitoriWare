using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceRing : MonoBehaviour {

	[SerializeField]
	float mAccel;

	[SerializeField]
	BroomRaceBackground background;


	void OnTriggerEnter2D(Collider2D other) {
		background.speedMultiplier = mAccel;
	}

}
