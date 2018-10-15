using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour {

    [Header("The thing to fly towards")]
    [SerializeField]
    private GameObject target;

    private Vector2 trajectory;

    private float speedMod;

	// Use this for initialization
	void Start () {
        speedMod = 1;
        this.trajectory = GetTrajectoryTowardsTarget();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.trajectory != null){
            this.transform.position = GetNewPosition();
        }
        this.trajectory = GetTrajectoryTowardsTarget();
    }

    Vector2 GetTrajectoryTowardsTarget() {
        return (target.transform.position - this.transform.position).normalized;
    }

    Vector2 GetNewPosition() {
        if (speedMod<20) {
            speedMod = speedMod + 0.1F;
        }
        return (Vector2)transform.position + (trajectory * Time.deltaTime * speedMod);
    }
}
