using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_WriggleScript : MonoBehaviour {

    public GameObject myHitbox;
    public GameObject wriggleCounter;
    public GameObject wriggleDead;
    public GameObject targetObject;
    public float speedIncrease = 2f;
    public float curveFactor = 0.5f;
    public float maxSpeed = 7f;
    public float startSpeed = 7f;
    public float stopTime = 1f;

    [SerializeField]
    private AudioClip deathSound1;

    [SerializeField]
    private AudioClip deathSound2;

    private bool isStopped = false;
    private Rigidbody2D rb2d;
    private float startScalex;
    private AudioSource buzzSource;
    

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        buzzSource = GetComponent<AudioSource>();
        buzzSource.time = Random.Range(0f, .5f);
        startScalex = transform.localScale.x;
        beginMovement();
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.GetComponent<BugSwat_InstantDestroy>() != null)
        {
            wriggleCounter.GetComponent<BugSwat_Counter>().wriggleCount -= 1;
            int tempRandom = Random.Range(0, 100);
            if (tempRandom > 50)
            {
                MicrogameController.instance.playSFX(deathSound1, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(0));
            }
            else
            {
                MicrogameController.instance.playSFX(deathSound2, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(0));
            }

            buzzSource.Stop();

            GameObject myDeath = Instantiate(wriggleDead);
            myDeath.transform.position = transform.position;
            myDeath.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        //prevent movement fix when stopped
        if (isStopped)
        {
            rb2d.velocity = new Vector2(0, 0);
            return;
        }

        //look left or right based on velocity
        if (rb2d.velocity.x > 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-startScalex, transform.localScale.y, transform.localScale.z);
        }
        if (rb2d.velocity.x < 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(startScalex, transform.localScale.y, transform.localScale.z);
        }

        //control out of bounds
		if (transform.position.x < -4.5f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x + speedIncrease, rb2d.velocity.y);
        }
        if (transform.position.x > 4.5f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x - speedIncrease, rb2d.velocity.y);
        }

        if (transform.position.y > 3.7f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y - speedIncrease);
        }

        if (transform.position.y < -3.7f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y + speedIncrease);
        }


        //simulate curves
        if (transform.position.x < targetObject.transform.position.x)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x + curveFactor, rb2d.velocity.y);
        }
        if (transform.position.x > targetObject.transform.position.x)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x - curveFactor, rb2d.velocity.y);
        }

        if (transform.position.y > targetObject.transform.position.y)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y - curveFactor);
        }

        if (transform.position.y < targetObject.transform.position.y)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y + curveFactor);
        }

        //limit speed
        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < -maxSpeed)
        {
            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.y > maxSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxSpeed);
        }
        if (rb2d.velocity.y < -maxSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -maxSpeed);
        }

        buzzSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
    }

    void beginMovement()
    {
        isStopped = false;
        float randomStop = Random.Range(1f, 1.5f) * 5f;
        Invoke("stopMovement", randomStop);

        int randomTemp = Random.Range(1, 4);

        if (randomTemp == 1)
        {
            rb2d.velocity = new Vector2(startSpeed, startSpeed);
        }
        if (randomTemp == 2)
        {
            rb2d.velocity = new Vector2(startSpeed, -startSpeed);
        }
        if (randomTemp == 3)
        {
            rb2d.velocity = new Vector2(-startSpeed, startSpeed);
        }
        if (randomTemp == 4)
        {
            rb2d.velocity = new Vector2(-startSpeed, -startSpeed);
        }
    }

    void stopMovement()
    {
        isStopped = true;
        rb2d.velocity = new Vector2(0, 0);

        float randomTemp = Random.Range(stopTime - 0.5f, stopTime + 0.5f);
        Invoke("beginMovement", randomTemp);
    }
}
