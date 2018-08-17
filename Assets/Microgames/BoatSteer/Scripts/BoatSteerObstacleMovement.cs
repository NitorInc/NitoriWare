using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerObstacleMovement : MonoBehaviour
{
	[SerializeField]
	private BoatSteerBoat boat;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.localPosition = transform.localPosition - (boat.getVelocity () * Time.deltaTime);
	}
}
