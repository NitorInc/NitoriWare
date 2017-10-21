using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeKnife : MonoBehaviour {
	Vector3 facingDirection;
	public bool isMoving;
	public bool isRotating;
	// Use this for initialization
	void Start () {
		isMoving = false;
		isRotating = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (isRotating) {
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (Vector3.forward, transform.position - facingDirection), Time.deltaTime);
		}

		if (isMoving) {
			GetComponent<Rigidbody2D> ().AddForce (-1.0f * transform.up * 10.0f);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground") {
			GetComponent<Rigidbody2D> ().simulated = false;
			isMoving = isRotating = false;
		}
	}

	public void SetFacing(Vector3 vec) {
		facingDirection = vec;
	}
}
