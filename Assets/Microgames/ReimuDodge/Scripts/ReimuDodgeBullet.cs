using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodgeBullet : MonoBehaviour
{

    [Header("The object the bullet flies towards")]
    [SerializeField]
    private GameObject target;
    [Header("The speed the bullet moves in")]
    [SerializeField]
    private float speed = 1.0f;

    private Vector2 trajectory;
    // Use this for initialization
    void Start()
    {
        trajectory = (target.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (trajectory != null)
        {
            transform.position = (Vector3)Vector2.MoveTowards((Vector2)transform.position, (Vector2)target.transform.position, Time.deltaTime * speed);
        }
    }
}
