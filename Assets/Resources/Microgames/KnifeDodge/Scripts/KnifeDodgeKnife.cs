using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeKnife : MonoBehaviour {
	Vector3 facingDirection;
	bool isMoving;
	bool isRotating;
	public float knifeSpeed = 20.0f;
	public float knifeRotationSpeed = 1.0f;
	// Use this for initialization
	void Start () {
		isMoving = false;
		isRotating = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (isRotating) {
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (Vector3.forward, transform.position - facingDirection), knifeRotationSpeed * Time.deltaTime);
		}

		if (isMoving) {
			isRotating = false;
			GetComponent<Rigidbody2D> ().AddForce (-1.0f * transform.up * knifeSpeed);
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

	public void SetMoving(bool moving) {
		isMoving = moving;
	}
}
