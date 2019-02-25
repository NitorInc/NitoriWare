using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePartsLimbTilt : MonoBehaviour {

	[SerializeField]
	private AnimationCurve speedToTilt;
	[SerializeField]
	private float smoothness;
	private Vector3 lastPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 velocity = transform.position - lastPosition;
		transform.eulerAngles = Vector3.forward * Mathf.LerpAngle(
			transform.eulerAngles.z,
			speedToTilt.Evaluate(velocity.x / Time.unscaledDeltaTime),
			Time.deltaTime * smoothness
		);
		lastPosition = transform.position;
	}
}
