using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{

    [Header("The player the bullet flies towards")]
    [SerializeField]
    private ReimuDodgePlayer target;
    [Header("The speed the bullet moves in")]
    [SerializeField]
    private float speed = 1.0f;

    [Header("Seconds until bullet starts homing on target")]
    [SerializeField]
    private float delay = 0f;
    [Header("Seconds until bullet stops homing on target")]
    [SerializeField]
    private float stop = 2.0f;
    private Vector2 trajectory;
    // Use this for initialization
    private float startTime;
    void Start(){
        startTime = Time.time;
    }
    float sinceStart(){
        return Time.time - startTime;
    }
    private void SetTrajectory()
    {
        if (target.alive && sinceStart() > delay && sinceStart() < stop)
        {
            trajectory = (target.transform.position - transform.position).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetTrajectory();
        if (trajectory != null)
        {
            float step = Time.deltaTime * speed;
            transform.position = (Vector2)transform.position + (step * trajectory);
        }
    }
}
