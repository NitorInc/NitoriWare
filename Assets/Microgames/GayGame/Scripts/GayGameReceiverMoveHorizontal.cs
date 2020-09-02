using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameReceiverMoveHorizontal : MonoBehaviour
{

    [SerializeField]
    private Transform receiverTransform;
    [SerializeField]
    private float xBound;
    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start ()
    {
        SetXPosition(Random.Range(-xBound, xBound));
	}
	
	void Update ()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            enabled = false;
            return;
        }

        var newXpos = GetXPosition() + (speed * Time.deltaTime);
        if (newXpos >= xBound)
            newXpos -= xBound * 2f;
        SetXPosition(newXpos);
    }

    void SetXPosition(float x) => receiverTransform.position = new Vector3(x, receiverTransform.position.y, receiverTransform.position.z);
    float GetXPosition() => receiverTransform.position.x;
}
