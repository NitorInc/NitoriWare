using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceBackground : MonoBehaviour {


	[SerializeField]
	float moveSpeed;

	public float speedMultiplier;



	void Start () { speedMultiplier = 1; }

	void Update () {
		transform.position += Vector3.left * moveSpeed * speedMultiplier * Time.deltaTime;
	}


}
