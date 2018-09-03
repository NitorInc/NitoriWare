using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_Scaler : MonoBehaviour {

    [SerializeField]
    private float scaleFactor = 8;

    [SerializeField]
    private float scaleSpeed = 8;

    private float initialScale;

    // Use this for initialization
    void Start () {
        initialScale = transform.localScale.x;
    }

	// Update is called once per frame
	void Update () {
        float newScale = ((1f / scaleFactor) + (Mathf.Sin(Time.time * scaleSpeed) / 5)) * initialScale * scaleFactor;
        transform.localScale = new Vector3(newScale, newScale, 1f);
    }
}
