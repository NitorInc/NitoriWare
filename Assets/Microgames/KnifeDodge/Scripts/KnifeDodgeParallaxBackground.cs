using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeParallaxBackground : MonoBehaviour {

	float speed = 1; // speed in meters per second

	void Update(){
		// move this object at frame rate independent speed:
		transform.position += new Vector3(1,0,0) * speed * Time.deltaTime;
	}

    public void SetSpeed(float spd) => speed = spd;
    public float GetSpeed() => speed;
}
