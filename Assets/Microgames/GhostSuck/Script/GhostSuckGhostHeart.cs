using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckGhostHeart : MonoBehaviour {
    [SerializeField]
    private bool touch, damageperiod, touchcheck, suck, inthezone, alive, acceldelay, settrajectorydeadactive, particlefired, justdid;
    [SerializeField]
    private float ghostlife, ghostdamage, damageinterval, speed, angle, acceleration, accel, movespeed, panicspeed, relaxspeed, deaddelay, diespeed, disable;
    [SerializeField]
    public float ghostsuckcount;
    [SerializeField]
    private GameObject body, ghost;
    [SerializeField]
    private Vector2 trajectory, deadtrajectory;
    [Header("TargetDead")]
    [SerializeField]
    private GameObject targetdead;
    [Header("Delay")]
    [SerializeField]
    private float delay = 1f;
    private Vector3 shrinking;
    private float add1, add2, sign1, sign2;

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MicrogameTag1")
        {
            if (suck == true && alive == true)
            {
                touch = true;
            }

            inthezone = true;
            movespeed = panicspeed;
        }
        if (collision.gameObject.tag == "MicrogameTag2" && alive == false)
        {
            if (particlefired == false)
            {
                ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
                particleSystem.Play();
            }
            Invoke("DisableObject", disable);
        }
    }
    void DisableObject()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);
        particlefired = true;
        GhostSuckGhostHeart ghostsuckGhostHeart = GetComponent<GhostSuckGhostHeart>();
        ghostsuckGhostHeart.enabled = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MicrogameTag1")
        {
            touch = false;
            inthezone = false;
            movespeed = relaxspeed;
        }
    }

    void Start() {
        touch = false;
        damageperiod = true;
        SetTrajectory();
        movespeed = relaxspeed;
        disable = 0.1f;
        particlefired = false;

    }
    void SetTrajectory()
    {
        add1 = Random.Range(0f, 2f);
        if (add1 > 1f)
        {
            sign1 = -1f;
        }
        else
        {
            sign1 = 1f;
        }
        add2 = Random.Range(0f, 2f);
        if (add1 > 1f)
        {
            sign2 = -1f;
        }
        else
        {
            sign2 = 1f;
        }
        trajectory = new Vector2(2f * sign1, Mathf.Round(Random.Range(0f, 2f)) * sign2);
        if (trajectory.x > 0f)
        {
            transform.Rotate(new Vector3(0, 180, 0));
        }           

    }
    void SetTrajectoryDead()
    {
        deadtrajectory = (targetdead.transform.position - transform.position).normalized;
        settrajectorydeadactive = true;
    }
    void plummetuntoDeath()
    {
        if (deadtrajectory != null)

        {
            Vector2 newPosition = (Vector2)transform.position + (deadtrajectory * diespeed * Time.deltaTime);
            transform.position = newPosition;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (alive == true)
        {
            updateMovement();

        }

        if (alive == false)
        {
            updateRotation();
            plummetuntoDeath();
            if (settrajectorydeadactive == true)
            {
                Invoke("SetTrajectoryDead", deaddelay);
                settrajectorydeadactive = false;
            }
            shrinking = transform.localScale;
            shrinking.x = shrinking.x * 0.96f;
            shrinking.y = shrinking.y * 0.96f;
            transform.localScale = shrinking;
        }

        if (Input.GetKey(KeyCode.Mouse0) && alive == true)
        {
            suck = true;
            if (inthezone == true)
            {
                touch = true;

            }
        }
        else
        {
            suck = false;
            touch = false;
        }
        if (touch == true)
        {
            if (damageperiod == true)
            {
                damageperiod = false;
                Invoke("TouchDamage", damageinterval);
            }

        }
        if (alive == true)
        {
            if (transform.position.x < -4.5f)
            {
                trajectory = new Vector2(-trajectory.x, trajectory.y);
                transform.Rotate(new Vector3(0, 180, 0));

            }
            if (transform.position.x > 4.5f)
            {
                trajectory = new Vector2(-trajectory.x, trajectory.y);
                transform.Rotate(new Vector3(0, 180, 0));
            }

            if (transform.position.y > 3.0f)
            {
                trajectory = new Vector2(trajectory.x, -trajectory.y);
            }

            if (transform.position.y < 0f)
            {
                trajectory = new Vector2(trajectory.x, -trajectory.y);
            }
        }



    }

    void TouchDamage()
    {

        if (ghostlife < 0)
        {
            alive = false;
            SetTrajectoryDead();
            ghostcountmodifier();
        }
        else
        {
            damageperiod = true;
            ghostlife -= ghostdamage;
        }
    }

    void updateRotation()
    {
        angle = body.transform.eulerAngles.z + (-1f * speed * Time.deltaTime * acceleration);
        body.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        if (accel < 5f && acceldelay == false)
        {
            Invoke("accelerateprotocol", 0.3f);
            acceldelay = true;

        }
    }
    void updateMovement()
    {
        Vector2 newPosition = (Vector2)transform.position + (trajectory * movespeed * Time.deltaTime);
        transform.position = newPosition;
    }

    void accelerateprotocol()
    {
        accel += 1f;
        acceleration += .5f;
        acceldelay = false;
    }
    void ghostcountmodifier()
    {
        ghost.BroadcastMessage("killaghost", ghost);
        movespeed = 0f;
    }


}
