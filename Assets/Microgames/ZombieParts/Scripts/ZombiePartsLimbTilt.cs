using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ZombiePartsLimb))]
public class ZombiePartsLimbTilt : MonoBehaviour {

	[SerializeField]
	private AnimationCurve speedToTilt;
	[SerializeField]
	private float smoothness;
	[SerializeField]
	private float slowSmoothness;


	private ZombiePartsLimb limb;
	private Vector3 lastPosition = Vector3.zero;
	private float initialAngle;


	public void SetInitialRotationAngle (float initialAngle) {
		this.initialAngle = initialAngle;
	}


	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
		limb = GetComponent<ZombiePartsLimb>();
	}

	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
		float activeSmoothness = limb.GetIsReturning() ? smoothness : slowSmoothness;

		transform.eulerAngles = Vector3.forward * Mathf.LerpAngle(
			transform.eulerAngles.z,
			initialAngle + speedToTilt.Evaluate(velocity.x / Time.unscaledDeltaTime),
			Time.deltaTime * activeSmoothness
		);

		lastPosition = transform.position;
	}

}
