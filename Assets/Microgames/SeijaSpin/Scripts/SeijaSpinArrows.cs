using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeijaSpinArrows : MonoBehaviour {

    private float currentScale;
    private Vector3 originalScaleValues;
    public bool flipped;

	// Use this for initialization
	void Start () {
        flipped = MathHelper.randomBool();
        if (flipped)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        originalScaleValues = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        currentScale = (float)(1.1f + Mathf.PingPong(Time.time * 0.5f, 0.2f));
        transform.localScale = new Vector3(originalScaleValues.x * currentScale, originalScaleValues.y * currentScale, originalScaleValues.z);
	}
}
