using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

	public float speed = 1; // speed in meters per second

	void Update(){
		// move this object at frame rate independent speed:
		transform.position += new Vector3(1,0,0) * speed * Time.deltaTime;
	}
}
