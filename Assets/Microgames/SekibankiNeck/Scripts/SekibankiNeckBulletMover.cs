using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekibankiNeckBulletMover : MonoBehaviour {

    [Header("Target")]
    [SerializeField]
    private GameObject target;

    [Header("Target2")]
    [SerializeField]
    private GameObject target2;

    [Header("Speed")]
    [SerializeField]
    private float speed = 1f;

    [Header("Delay")]
    [SerializeField]
    private float delay = 1f;

    private Vector2 trajectory;

	// Use this for initialization
	void Start () {

        Invoke("SetTrajectory", 0f);

    
	}
	
	// Update is called once per frame
	void Update () {
        if (trajectory != null)

        {
            Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }
	}


    void SetTrajectory()
    {
        trajectory = (target.transform.position - transform.position).normalized;

        Invoke("SetTrajectory2", delay);
    }

    void SetTrajectory2()
    {
        trajectory = (target2.transform.position - transform.position).normalized;

        Invoke("SetTrajectory", delay);
    }
}
