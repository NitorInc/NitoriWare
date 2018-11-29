using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookMovement : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Vector2 mouseRotateVelocity;
    [SerializeField]
    private Vector2 yRotateBounds;
#pragma warning restore 0649

	void Awake()
	{
        transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, Random.Range(-180f, 180f), 0f));
    }
	
	void Update()
    {
        if (MicrogameController.instance.getVictoryDetermined())
        {
            enabled = false;
            return;
        }

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        float x = eulerRotation.x;
        while (x > 90f)
        {
            x -= 360f;
        }
        x -= Input.GetAxis("Mouse Y") * mouseRotateVelocity.y;
        x = Mathf.Clamp(x, yRotateBounds.x, yRotateBounds.y);

        float y = eulerRotation.y;
		y += Input.GetAxis("Mouse X") * mouseRotateVelocity.x;
		
        transform.rotation = Quaternion.Euler(new Vector3(x, y, 0f));
    }
}
