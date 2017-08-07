using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookTarget : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private float spinSpeed;
#pragma warning restore 0649

    private float floorDistance;
    private float floorAngle;

	void Start()
	{
        Vector2 fromNitori = new Vector2(transform.position.x, transform.position.z);
        floorDistance = fromNitori.magnitude;
        floorAngle = fromNitori.getAngle();

    }
	
	void Update()
	{
        updateAngle();
        if (spinSpeed > 0f)
            updateSpin();
	}

    void updateSpin()
    {
        //Vector2 fromNitori = new Vector2(transform.position.x, transform.position.z);
        floorAngle += spinSpeed * Time.deltaTime;
        Vector2 fromNitori = MathHelper.getVector2FromAngle(floorAngle, floorDistance);
        transform.position = new Vector3(fromNitori.x, transform.position.y, fromNitori.y);
    }

    void updateAngle()
    {
        Transform camTransform = Camera.main.transform;
        transform.rotation = camTransform.rotation;
        //Vector3 cameraEulers = Camera.rotation.eulerAngles;
        //cameraEulers.Scale(new Vector3(.5f, 1f, 1f));
        //cameraEulers = new Vector3(cameraEulers.x * .5f, cameraEulers.y, cameraEulers.z);
        //transform.rotation = Quaternion.Euler(cameraEulers);
    }
}
