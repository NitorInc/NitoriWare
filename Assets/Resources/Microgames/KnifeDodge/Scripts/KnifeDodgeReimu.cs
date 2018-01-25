using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeReimu : MonoBehaviour {
	public float speed = 1; // speed in meters per second

	// Use this for initialization
	void Start () {
		
	}

	void Update(){
		Vector3 moveDir = Vector3.zero;
		moveDir.x = Input.GetAxis("Horizontal"); // get result of AD keys in X
		moveDir.z = Input.GetAxis("Vertical"); // get result of WS keys in Z
		// move this object at frame rate independent speed:
		transform.position += moveDir * speed * Time.deltaTime;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Hazard") {
			Kill ();
			MicrogameController.instance.setVictory(false, true);
		}
	}

	public void Kill() {
		GetComponent<BoxCollider2D> ().size = new Vector2(0,0);
		transform.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 10.0f);
		transform.GetComponent<Rigidbody2D> ().angularVelocity = 90.0f;
	}
}
