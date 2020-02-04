using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatBilliardsBall : MonoBehaviour
{
    [SerializeField]
    float speed = 1;
    [SerializeField]
    Rigidbody rigidbody;
    [SerializeField]
    HatBilliardsHat hat;
    [SerializeField]
    private LineRenderer guideLine;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float stopXLaunch;
    [SerializeField]
    private float stopYLaunch;

    [SerializeField]
    private float launchDelay = .2f;

    bool mouseClicked;
    bool launched;
    Vector3 targetPosition;
    int currentIndex = 0;

    public delegate void HitDelegate();
    public static HitDelegate onHit;
    Vector3 facing;

    private void Awake()
    {
        onHit = null;
    }

    void launch()
    {
        launched = true;
    }

    void Update ()
    {
        if (!mouseClicked && Input.GetMouseButtonDown (0))
        {
            mouseClicked = true;
            targetPosition = guideLine.GetPosition(1);
            //transform.forward = targetPosition - transform.position;
            facing = targetPosition - transform.position;
            Invoke("launch", launchDelay);
            onHit();
        }

        else if (launched)
        {
            transform.position = Vector3.MoveTowards (transform.position, targetPosition, Time.deltaTime * speed);
            if (transform.position == targetPosition) //Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                currentIndex++;
                if (currentIndex < guideLine.positionCount)
                {
                    targetPosition = guideLine.GetPosition(currentIndex);
                    facing = targetPosition - transform.position;
                    //transform.forward = targetPosition - transform.position;
                }
                else
                {
                    //rigAnimator.SetTrigger("Stop");
                    var rb = GetComponent<Rigidbody>();
                    var stopLaunchForce = (facing * -1f).normalized * stopXLaunch;
                    stopLaunchForce.y = stopYLaunch;
                    rb.isKinematic = false;
                    rb.AddForce(stopLaunchForce);
                    enabled = false;

                    MicrogameController.instance.setVictory(false);
                }
            }
        }
    }

    void OnTriggerEnter (Collider col)
    {
        if (col.gameObject.name == "Hat")
        {
            hat.PingAway ();
            gameObject.SetActive (false);

            MicrogameController.instance.setVictory(true);
        }
    }
}
