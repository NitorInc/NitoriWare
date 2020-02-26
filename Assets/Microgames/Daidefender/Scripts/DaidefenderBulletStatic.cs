using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaidefenderBulletStatic : MonoBehaviour {

    private Vector3 posA;

    private Vector3 posB;

    private Vector3 nexPos;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform childTransform;

    [SerializeField]
    private Transform transformB;

	// Use this for initialization
	void Start () {
        posA = childTransform.localPosition;
        posB = transformB.localPosition;
        nexPos = posB;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nexPos, speed * Time.deltaTime);
        if (Vector3.Distance(childTransform.localPosition,nexPos) <= 0.1)
        {
            CallBack();
        }
    }

    private void CallBack()
    {
        nexPos = nexPos != posA ? posA : posB;
    }
}
