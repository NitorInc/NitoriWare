using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{

    [SerializeField]
    private float speed = 1, delay = 1;

    private Vector2 trajectory;

    void Start()
    {
        Invoke("SetTrajectory", delay);
    }

    void SetTrajectory()
    {
        trajectory = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
    }

    void Update()
    {
        transform.position += (Vector3)trajectory * Time.deltaTime * speed;
    }
}
