using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallCollisionObserver : MonoBehaviour
{
    protected Collider2D innerArea;

    protected Collider2D ballCollider;
    protected Rigidbody2D ballPhysics;

    protected bool fired = false;
    public bool Fired
    {
        get
        {
            return fired;
        }
        set
        {}
    }

    protected virtual void Start()
    {
        innerArea = GetComponent<CircleCollider2D>();

        var ballGo = GameObject.Find("Ball");
        ballCollider = ballGo.GetComponent<CircleCollider2D>();
        ballPhysics = ballGo.GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    public virtual void OnTriggerStay2D(Collider2D other)
    {
        if (!fired && ballPhysics.velocity.y < 0 && other == ballCollider)
        {
            fired = true;
            other.gameObject.GetComponent<BeachBallZSwitcher>().Switch();

            MicrogameController.instance.setVictory(victory: true, final: true);
        }
    }
}
