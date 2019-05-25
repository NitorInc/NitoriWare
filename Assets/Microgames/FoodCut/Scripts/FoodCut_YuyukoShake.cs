using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCut_YuyukoShake : MonoBehaviour {
    [SerializeField]
    private float offsetX;

    [SerializeField]
    private float offsetY;

    [SerializeField]
    private float shakeAmt;
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = Random.insideUnitCircle * (Time.deltaTime * shakeAmt);
        newPos.z = transform.position.z;
        newPos.x += offsetX;
        newPos.y += offsetY;
        transform.position = newPos;
    }
}
