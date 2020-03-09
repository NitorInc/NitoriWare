using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceBackground : MonoBehaviour {

	void Update () {
		transform.position += Vector3.left
            * BroomRaceBackgroundSpeed.instance.BaseSpeed
            * BroomRaceBackgroundSpeed.instance.SpeedMult
            * Time.deltaTime;
	}


}
