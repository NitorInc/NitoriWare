using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameSenderMovement : MonoBehaviour
{
    [SerializeField]
    private float scalePerY = .25f;
    [SerializeField]
    private float maxY = 3f;
    [SerializeField]
    private float anglePerX = 5f;

    private Vector3 startPosition;
    private Vector3 initialScale;

	void Start ()
    {
        startPosition = transform.position;
        initialScale = transform.localScale;
	}
	
	void LateUpdate ()
    {
        if (transform.position.y > maxY)
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        float currentScale = 1f + (transform.position.y - startPosition.y) * scalePerY;
        transform.localScale = initialScale * currentScale;
        float currentAngle = (startPosition.x - transform.position.x) * anglePerX;
        transform.localEulerAngles = Vector3.forward * currentAngle;
	}
}
