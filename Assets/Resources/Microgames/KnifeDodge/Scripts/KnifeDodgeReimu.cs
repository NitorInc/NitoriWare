using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeReimu : MonoBehaviour {
	public float fSpeed = 1; // speed in meters per second
	public float fAnimSpeed = 1f;
	public float fAnimSpeedStopped = 0.25f;

	bool bIsKilled;
	// Use this for initialization
	void Start () {
		bIsKilled = false;
	}

	void Update(){
		Vector3 moveDir = Vector3.zero;
	
		if (!bIsKilled) {
			moveDir.x = Input.GetAxisRaw ("Horizontal"); // get result of AD keys in X
			moveDir.z = Input.GetAxisRaw ("Vertical"); // get result of WS keys in Z
		}

		if (moveDir == Vector3.zero) {
			GetComponent<Animator> ().speed = fAnimSpeedStopped;
		} else {
			GetComponent<Animator> ().speed = fAnimSpeed;
		}

		// move this object at frame rate independent speed:
		transform.position += moveDir * fSpeed * Time.deltaTime;

		// if (Input.GetButtonDown ("Fire1")) Kill();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "KnifeDodgeHazard") {
			Kill ();
			MicrogameController.instance.setVictory(false, true);
		}
	}

	public void Kill() {
		bIsKilled = true;
		GetComponent<BoxCollider2D> ().size = new Vector2(0,0);
		transform.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 10.0f);
		transform.GetComponent<Rigidbody2D> ().angularVelocity = 80.0f;
	}
}
