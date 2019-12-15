using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasenPetsHands : MonoBehaviour {

    [SerializeField]
    private float ySpeed = 10f;
    [SerializeField]
    private float yExtents = 5f;

	void Start () {
		
	}

	void update () {
		
	}

	void LateUpdate () {
		UpdatePosition ();
	}

	//Basically the same as FollowCursor, but only affects the y-coordinate.
	void UpdatePosition() {
        var direction = 0f;
        if (Input.GetKey(KeyCode.UpArrow))
            direction += 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            direction -= 1f;

        if (direction != 0f)
            transform.moveTowards(new Vector3(transform.position.x, yExtents * direction, transform.position.z), ySpeed);
    }
}
