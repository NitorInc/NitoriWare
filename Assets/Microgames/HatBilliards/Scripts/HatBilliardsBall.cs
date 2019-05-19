using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatBilliardsBall : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    [SerializeField]
    Transform firstTarget;
    [SerializeField]
    Rigidbody rigidbody;
    [SerializeField]
    HatBilliardsHat hat;

    bool fire;
    bool hit;
    Vector3 targetPosition;

    void Update ()
    {
        if (!fire && Input.GetMouseButtonDown (0))
        {
            targetPosition = firstTarget.localPosition;
            print (targetPosition);
            fire = true;
        }

        if (fire)
        {
            transform.localPosition = Vector3.MoveTowards (transform.localPosition, targetPosition, Time.deltaTime * speed);
            if (!hit && transform.localPosition == targetPosition) //Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                targetPosition = new Vector3 (0, targetPosition.y, targetPosition.z * 2);
                print (targetPosition);
                hit = true;
            }
        }
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "Hat")
        {
            hat.PingAway ();
            gameObject.SetActive (false);
        }
    }
}
