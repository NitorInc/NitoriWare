using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeKnife : MonoBehaviour {
	Vector3 facingDirection;
    int state;

    public float knifeSpeed = 20.0f;
	public float knifeRotationSpeed = 1.0f;


    public enum KnifeState
    {
        FLYING_IN,
        STOP_AND_ROTATE,
        MOVING_TO_GROUND,
    }

	// Use this for initialization
	void Start () {
        state = (int)KnifeState.FLYING_IN;

    }
	
	// Update is called once per frame
	void Update () {

        switch (state)
        {
            case (int)KnifeState.FLYING_IN:
                GetComponent<Rigidbody2D>().AddForce(-0.5f * transform.up * knifeSpeed);
                break;

            case (int)KnifeState.STOP_AND_ROTATE:
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, transform.position - facingDirection), knifeRotationSpeed * Time.deltaTime);
                break;

            case (int)KnifeState.MOVING_TO_GROUND:
                GetComponent<Rigidbody2D>().AddForce(-1.0f * transform.up * knifeSpeed);
                break;
        }
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "KnifeDodgeGround") {
			GetComponent<Rigidbody2D> ().simulated = false;
            CameraShake.instance.setScreenShake(.15f);
            CameraShake.instance.shakeCoolRate = .5f;
        }
	}

	public void SetFacing(Vector3 vec) {
		facingDirection = vec;
	}

    public void SetState(int stateNumber)
    {
        state = stateNumber;
    }
}
