using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceBackground : MonoBehaviour {
    [SerializeField]
    private float localMult = 1f;
    [SerializeField]
    private float wrapDistance;
    [SerializeField]
    private bool independent = false;

	void Update () {
		transform.position += Vector3.left
            * (independent ? 1f : BroomRaceBackgroundSpeed.instance.BaseSpeed)
            * (independent ? 1f : BroomRaceBackgroundSpeed.instance.SpeedMult)
            * localMult
            * Time.deltaTime;

        if (transform.position.x < -wrapDistance)
            transform.position += Vector3.right * wrapDistance;
        else if (transform.position.x > 0f)
            transform.position += Vector3.left * wrapDistance;

    }


}
