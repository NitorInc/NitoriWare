using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatRigBehaviour : MonoBehaviour
{
    [SerializeField]
    private BoatSteerBoat boatObject;
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

    void Update() {
		boat.transform.localRotation = Quaternion.Euler(0, 0, -boatObject.animationFactor * 15f);
		boatSplash.transform.localRotation = Quaternion.Euler(0, 0, -boatObject.animationFactor * 10f);
		murasaSteeringArm.transform.localRotation = Quaternion.Euler(0, 0, (-boatObject.animationFactor)*murasaSteeringArmAngle);
    }
}