using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerBoat : MonoBehaviour
{
	[SerializeField]
	private float speed = 10;


	[SerializeField]
	private float headingPerSec = 90;

	[SerializeField]
	private float maxHeading;

	private float heading;

	[SerializeField]
	private GameObject rig;

	// Instead of doing this programatically I should probably use the animation system
	// But hey, if it works, why knock it?
	[SerializeField]
	private GameObject boat;

	[SerializeField]
	private GameObject boatSplash;

	[SerializeField]
	private GameObject murasa;

	[SerializeField]
	private GameObject murasaSteeringArm;

	[SerializeField]
	private float murasaSteeringArmAngle;

	private bool crashed = false;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (crashed) {
			rig.transform.localPosition += Time.deltaTime * (new Vector3 (0, -1, 0));
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

		transform.localPosition += new Vector3 (
			Mathf.Sin (Mathf.Deg2Rad * heading) * speed * Time.deltaTime, 
			0,
			Mathf.Cos (Mathf.Deg2Rad * heading) * speed * Time.deltaTime
		);
		boat.transform.localRotation = Quaternion.Euler (0, 0, -heading / 4f);
		boatSplash.transform.localRotation = Quaternion.Euler (0, 0, -heading / 9f);
		murasaSteeringArm.transform.localRotation = Quaternion.Euler (0, 0, (-heading/maxHeading)*murasaSteeringArmAngle);

		Camera.main.transform.position = transform.position + new Vector3(0f, 1.732f, -1f);
		Camera.main.transform.rotation = Quaternion.Euler(15, 0, -heading/8f);
	}

	void OnTriggerEnter (Collider other)
	{
		MicrogameController.instance.setVictory (false, true);
		crashed = true;
	}
}
