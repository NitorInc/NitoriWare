using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HecShapes_ShapeMatch : MonoBehaviour {
		// Use this for initialization
	private string headColor;
	private string planetColor;

	void Start () {
		if (gameObject.name.Equals ("HecShapes_HecHat0")) {
			headColor = "HecShapesHecPlanet0";
		} else {
			if (gameObject.name.Equals ("HecShapes_HecHat1")) {
				headColor = "HecShapesHecPlanet1";
			} else {
				if (gameObject.name.Equals ("HecShapes_HecHat2")) {
					headColor = "HecShapesHecPlanet2";
				}
			}
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		//to check if the planet matches the head
		planetColor = other.gameObject.name;
		if (planetColor == headColor) {
			print ("The planet is in!");
		}

	}
}


