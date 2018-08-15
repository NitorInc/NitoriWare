using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekibankiNeckBulletTriangle : MonoBehaviour
{

    [Header("Target")]
    [SerializeField]
    private GameObject target;

    [Header("Target2")]
    [SerializeField]
    private GameObject target2;

    [Header("Target3")]
    [SerializeField]
    private GameObject target3;

    [Header("Speed")]
    [SerializeField]
    private float speed = 1f;

    [Header("Delay")]
    [SerializeField]
    private float delay = 1f;

    private float Sekibanki3Right = 2f;

    private Vector2 trajectory;

    // Use this for initialization
    void SekibankiNeck3ChangeDirection()
    {
       if (Sekibanki3Right > 0f)
        {
         Sekibanki3Right = 0f;
        }
        
        
    }

    void Start()
    {
        
        Invoke("SetTrajectory", 0.1f);

    }


    // Update is called once per frame
    void Update()
    {
        if (trajectory != null)

        {
            Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosition;
        }
    }


    void SetTrajectory()
    {
        if (Sekibanki3Right > 0f)
        {
            trajectory = (target.transform.position - transform.position).normalized;
            
        }
        else
        {
            trajectory = (target2.transform.position - transform.position).normalized;

        }
        
        Invoke("SetTrajectory2", delay);
    }

    void SetTrajectory2()
    {

        if (Sekibanki3Right > 0f)
        {
            trajectory = (target2.transform.position - transform.position).normalized;

        }
        else
        {
            trajectory = (target.transform.position - transform.position).normalized;

        }



        Invoke("SetTrajectory3", delay);
    }

    void SetTrajectory3()
    {
        if (Sekibanki3Right > 0f)
        {
            trajectory = (target3.transform.position - transform.position).normalized;
            
        }
        else
        {
            trajectory = (target3.transform.position - transform.position).normalized;
        }

        Invoke("SetTrajectory", delay);
    }
}

