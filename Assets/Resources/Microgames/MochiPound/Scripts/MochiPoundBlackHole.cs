using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MochiPoundBlackHole : MonoBehaviour {

    public MochiPoundController controller;

    public float targetScale = 3.0f;
    float startScale = 0.34f;
    public float scalePerHit {
        get {
            return targetScale / controller.hitGoal;
        }
    }
	// Use this for initialization
	void Start () {
        startScale = transform.localScale.x;
        controller.onHit += ScaleBlackHole;
	}

    void ScaleBlackHole() {
        var x = transform.localScale.x;
        Vector3 scale = new Vector3(x, x, startScale);
        scale.x += scalePerHit;
        scale.y += scalePerHit;
        transform.localScale = scale;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
