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

    private int direction;

	// Use this for initialization
	void Start ()
    {
        SetXPosition(Random.Range(-xBound, xBound));
        direction = MathHelper.randomBool() ? 1 : -1;
	}
	
	void Update ()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            enabled = false;
            return;
        }

        var newXpos = GetXPosition() + (speed * Time.deltaTime * direction);
        if (direction > 0 && newXpos >= xBound)
        {
            newXpos = xBound;
            direction *= -1;
        }
        else if (direction < 0 && newXpos <= -xBound)
        {
            newXpos = -xBound;
            direction *= -1;
        }
        SetXPosition(newXpos);
    }

    void SetXPosition(float x) => receiverTransform.position = new Vector3(x, receiverTransform.position.y, receiverTransform.position.z);
    float GetXPosition() => receiverTransform.position.x;
}
