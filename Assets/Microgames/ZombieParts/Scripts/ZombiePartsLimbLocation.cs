using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePartsLimbLocation : MonoBehaviour {
	
	[SerializeField]
	private string correctLimbTag = "";
	
	private void OnCollisionEnter2D(Collision2D other) {
		ZombiePartsLimb zombiePartsLimb = other.gameObject.GetComponent<ZombiePartsLimb>();
		if (zombiePartsLimb != null && other.gameObject.tag == correctLimbTag) {
			zombiePartsLimb.inCorrectPosition = true;
			zombiePartsLimb.correctPosition = transform.position;
		}
	}

	private void OnCollisionExit2D(Collision2D other) {
		ZombiePartsLimb zombiePartsLimb = other.gameObject.GetComponent<ZombiePartsLimb>();
		if (zombiePartsLimb != null && other.gameObject.tag == correctLimbTag) {
			zombiePartsLimb.inCorrectPosition = false;
			zombiePartsLimb.correctPosition = Vector3.zero;
		}
	}
}
