using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_WriggleScript : MonoBehaviour {

    public GameObject myHitbox;
    public float speedIncrease = 2f;
    public float curveFactor = 0.5f;
    public float maxSpeed = 7f;
    public float startSpeed = 7f;
    public float stopTime = 1f;

    private bool isStopped = false;
    private Rigidbody2D rb2d;
    

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        beginMovement();
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.GetComponent<BugSwat_InstantDestroy>() != null)
        {
            MicrogameController.instance.setVictory(true, true);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        //prevent movement fix when stopped
        if (isStopped)
        {
            rb2d.velocity = new Vector2(0, 0);
            return;
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
        if (transform.position.x < 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x + curveFactor, rb2d.velocity.y);
        }
        if (transform.position.x > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x - curveFactor, rb2d.velocity.y);
        }

        if (transform.position.y > 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y - curveFactor);
        }

        if (transform.position.y < 0)
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
