using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerBoat : MonoBehaviour
{
	[SerializeField]
	public float speed = 16;

    [SerializeField]
    public float maxLateralSpeedFactor = 0.75f;

    private float lateralSpeed = 0;

	[SerializeField]
	float lateralAcceleration = 0.50f;

	[SerializeField]
	private float lateralDeceleration = 0.5f;

	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Camera backgroundCamera;

	[SerializeField]
	private Animator rigAnimator;

    [SerializeField]
    private float cameraTiltMult = 5f;

    private bool crashed = false;

	private Rigidbody rb;

    private void Awake()
    {
		rb = GetComponent<Rigidbody>();
    }

    public float animationFactor {
		get {
			return lateralSpeed / (speed * maxLateralSpeedFactor);
		}
	}

	void Update ()
	{
		if (crashed) {
			return;
		}
		bool left = Input.GetKey(KeyCode.LeftArrow);
		bool right = Input.GetKey(KeyCode.RightArrow);

		// Lmao euler integration
		if (left == right) {
			// Neutral input
			lateralSpeed -= lateralSpeed * lateralDeceleration * Time.deltaTime;
		} else {
			float maxLateralSpeed = speed*maxLateralSpeedFactor;
			float accel = speed * lateralAcceleration * Time.deltaTime;

			lateralSpeed += right ? accel : -accel;

			// Some bonus countersteer just ofr you Gman8r <3
			if ((right && lateralSpeed < 0 ) || (left && lateralSpeed > 0)) {
				lateralSpeed += accel * (-lateralSpeed/maxLateralSpeed);
			} 

			if (lateralSpeed > maxLateralSpeed) {
				lateralSpeed = maxLateralSpeed;
			} else if (lateralSpeed < -maxLateralSpeed) {
				lateralSpeed = -maxLateralSpeed;
			}
		} 


		backgroundCamera.transform.localPosition = new Vector3(animationFactor/10f, 0, 0);

		float cameraTilt = -animationFactor*cameraTiltMult;
		backgroundCamera.transform.localRotation = Quaternion.Euler(0, 0, cameraTilt);
		mainCamera.transform.localRotation = Quaternion.Euler(0, 0, cameraTilt);
	}

    private void FixedUpdate()
	{
		var moveSpeed = new Vector3(
			lateralSpeed,
			0,
			speed
		);
		print(moveSpeed);
		rb.velocity = moveSpeed;
	}

    private void crash() {
		if (crashed == true) {
			return;
		}
		crashed = true;

        rigAnimator.SetTrigger("Crash");
		rb.velocity = Vector3.zero;
		enabled = false;
        
		// TODO: Create sinking rig
	}

	void OnTriggerEnter (Collider other)
	{
		MicrogameController.instance.setVictory(false, true);

		crash();
	}
}
