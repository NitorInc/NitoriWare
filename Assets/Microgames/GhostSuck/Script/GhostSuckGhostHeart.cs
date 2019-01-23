using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckGhostHeart : MonoBehaviour {
    [SerializeField]
    private bool touch, damageperiod, touchcheck, suck, inthezone, alive, acceldelay, settrajectorydeadactive, particlefired, justdid;
    [SerializeField]
    private float ghostlife, ghostdamage, damageinterval, speed, angle, acceleration, accel, movespeed, panicspeed, relaxspeed, deaddelay, diespeed;
    [SerializeField]
    public float ghostsuckcount;
    [SerializeField]
    private GameObject body, ghost;
    [SerializeField]
    private Vector2 trajectory, deadtrajectory;
    [Header("TargetDead")]
    [SerializeField]
    private GameObject targetdead, bakebakerig;
    [Header("Delay")]
    [SerializeField]
    private float delay = 1f;
    private Vector3 shrinking;
    private float add1, add2, sign1, sign2;
    [SerializeField]
    private Animator ghostAnimator;
    [SerializeField]
    private AudioClip ghostPop;

    // Collider2D stuff, determines whether a particular ghost is under mouse hitbox
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (particlefired == false)
        {
            if (collision.gameObject.tag == "MicrogameTag1")
            {

                if (suck == true && alive == true)
                {
                    touch = true;
                }

                inthezone = true;
                movespeed = panicspeed;
                Animator ghostAnimator = GetComponentInChildren<Animator>();
                ghostAnimator.Play("BakeBakePanic");
            }
            // interacts with hitbox on vacuum nozzle when dead, creates particles and invokes disableobject
            if (collision.gameObject.tag == "MicrogameTag2" && alive == false)
            {
                if (particlefired == false)
                {
                    ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
                    particleSystem.Play();
                    Invoke("DisableObject", 0.1f);
                }

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (particlefired == false)
        {
            if (collision.gameObject.tag == "MicrogameTag1")
        {
            touch = false;
            inthezone = false;
            movespeed = relaxspeed;
            Animator ghostAnimator = GetComponentInChildren<Animator>();
            ghostAnimator.Play("BakeBakeMove");
        }
        }
           
    }
    //sets a boolean check that disables all update stuff
    void DisableObject()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);
        particlefired = true;
        MicrogameController.instance.playSFX(ghostPop, volume: 1.5f, pitchMult: 2f, panStereo: AudioHelper.getAudioPan(transform.position.x));
    }
    
    void Start() {
        touch = false;
        damageperiod = true;
        SetTrajectory();
        movespeed = relaxspeed;
        particlefired = false;
    }
    //randomly selects an angled initial trajectory for the ghost to follow
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
    //sets a trajectory to the vacuum nozzle, triggered once ghost is defeated
    void SetTrajectoryDead()
    {
        deadtrajectory = (targetdead.transform.position - transform.position).normalized;
        settrajectorydeadactive = true;
    }
    //moves ghost to vacuum nozzle
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
        if (particlefired == false)
        {
            if (alive == true)
            {
                updateMovement();

            }
            //shrinks the ghost
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
                shrinking.x = shrinking.x * 0.97f;
                shrinking.y = shrinking.y * 0.97f;
                transform.localScale = shrinking;
            }
            //periodically decreases ghost life if ghost is both under mouse and mouse is pressed
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
            //redirects ghost upon reaching intended bounds
            if (alive == true)
            {
                if (transform.position.x < -4.5f)
                {
                    transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y);
                    trajectory = new Vector2(-trajectory.x, trajectory.y);
                    transform.Rotate(new Vector3(0, 180, 0));

                }
                if (transform.position.x > 4.5f)
                {
                    transform.position = new Vector2(transform.position.x - 0.1f, transform.position.y);
                    trajectory = new Vector2(-trajectory.x, trajectory.y);
                    transform.Rotate(new Vector3(0, 180, 0));
                }

                if (transform.position.y > 3.0f)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 0.1f);
                    trajectory = new Vector2(trajectory.x, -trajectory.y);
                }

                if (transform.position.y < 0f)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
                    trajectory = new Vector2(trajectory.x, -trajectory.y);
                }
            }


        }
    }
    //periodically reduces ghost life until it reaches zero to which it switches alive movement trajectory to dead ghost trajectory among other functions
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
            rattle1();
        }
    }
    //makes ghost shake when taking damage
    void rattle1()
    {
        bakebakerig.transform.position = new Vector2(transform.position.x - 0.05f, transform.position.y);
        Invoke("rattle2", 0.1f);
    }

    void rattle2()
    {
        bakebakerig.transform.position = new Vector2(transform.position.x + 0.05f, transform.position.y);
    }
    //makes ghost spin when being sucked up
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
    //accelerates spinning of dead ghost
    void accelerateprotocol()
    {
        accel += 1f;
        acceleration += .5f;
        acceldelay = false;
    }
    //communicates this ghost's death to a counting game object
    void ghostcountmodifier()
    {
        ghost.BroadcastMessage("killaghost", ghost);
        movespeed = 0f;
    }


}
