using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDodgeHilt : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = new Color(Random.Range(0.0f, 1.0f),Random.Range(0f, 1.0f),Random.Range(0f, 1.0f),Random.Range(0.5f, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
