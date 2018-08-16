using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerBoat : MonoBehaviour
{
	private Vector3 velocity = new Vector3 ();

	private bool crashed = false;
	private float speed = 10;
	private float heading = 0;
	private float headingPerSec = 90;
	private float maxHeading = 60;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (crashed) {
			velocity.Set (0, 0, 0);
			transform.localRotation = Quaternion.identity;
			transform.localPosition += Time.deltaTime * (new Vector3 (0, -1, 0));
	
			return;
		}
		bool left = Input.GetKey (KeyCode.LeftArrow);
		bool right = Input.GetKey (KeyCode.RightArrow);

		float headingDelta = headingPerSec * Time.deltaTime;

		if (left == right) {
			// neutral steering - return to straight
			if (heading > 0) {
				heading = (heading < headingDelta ? 0 : heading - headingDelta);
			} else {
				heading = (heading > -headingDelta ? 0 : heading + headingDelta);
			}
		} else {
			headingDelta = right ? headingDelta : -headingDelta;
			heading = Mathf.Clamp (heading + headingDelta, -maxHeading, maxHeading);
		} 

		velocity.Set (
			Mathf.Sin (Mathf.Deg2Rad * heading) * speed, 
			0,
			Mathf.Cos (Mathf.Deg2Rad * heading) * speed
		);
		transform.localRotation = Quaternion.AngleAxis (-heading / 4f, Vector3.forward);
	}

	public Vector3 getVelocity ()
	{
		return velocity;
	}

	void OnTriggerEnter (Collider other)
	{
		// Lame hack: Move object back to avoid appearance of clipping through boat
		other.gameObject.transform.localPosition += velocity;
		MicrogameController.instance.setVictory (false, true);
		crashed = true;
	}
}
