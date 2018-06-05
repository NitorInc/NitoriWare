using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace GiveANameSpace

public class ReimuDodgeBullet : MonoBehaviour
{
    [Header("The thing to fly towards")]
    [SerializeField]
    private GameObject target;

    [Header("How fast da bullet")]
    [SerializeField]
    private float speed = 1f;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay = 1f;

    private Vector2 trajectory;

    void Start()
    {
        Invoke("SetTrajectory", delay);
    }

    void Update()
    {
        if (trajectory != null)
        {
            transform.position = (Vector2)transform.position + trajectory * Time.deltaTime * speed;
        }
    }

    void SetTrajectory()
    {
        trajectory = (target.transform.position - transform.position).normalized;
    }
}
