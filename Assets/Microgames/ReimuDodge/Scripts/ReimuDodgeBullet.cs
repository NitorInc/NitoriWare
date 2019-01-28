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

    private Vector2 trajectory;
    // Use this for initialization
    private void SetTrajectory()
    {
        if (target.alive)
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
