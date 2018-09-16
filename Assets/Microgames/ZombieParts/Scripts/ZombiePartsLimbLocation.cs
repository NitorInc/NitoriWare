using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePartsLimbLocation : MonoBehaviour {

	[SerializeField]
	private string correctLimbTag = "";
	public bool completed = false;
	
	private void OnCollisionStay2D(Collision2D other) {
		if (completed && other.gameObject.tag == correctLimbTag) {
			MouseGrabbable otherGrabbable = other.gameObject.GetComponent<MouseGrabbable>();
			if (otherGrabbable != null && !otherGrabbable.grabbed) {
				other.gameObject.transform.position = transform.position;
				otherGrabbable.enabled = false;
				completed = true;
				// MicrogameController.instance.setVictory(true, true);
			}
		}
	}
}
