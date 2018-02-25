using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSwat_DeadWriggle : MonoBehaviour {

    public GameObject particlePrefab;

    private int timesLooped = 0;
    private float startX;
    private float twitchX = 0.05f;
    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;

        Instantiate(particlePrefab, transform.position, Quaternion.identity);
        
        Invoke("startUp", 0.01f);
        Invoke("Twitch", 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void startUp()
    {
        startX = transform.position.x;
    }

    void Twitch()
    {
        if (timesLooped < 25)
        {
            float newX = twitchX + startX;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            timesLooped += 1;
            twitchX *= -1;
            Invoke("Twitch", 0.01f);
        }
        else
        {
            rb2d.gravityScale = 10;
            int directionDecider = Random.Range(0, 100);
            if (directionDecider > 50)
            {
                rb2d.velocity = new Vector2(5, 2);
                rb2d.angularVelocity = -150;
            }
            else
            {
                rb2d.velocity = new Vector2(-5, 2);
                rb2d.angularVelocity = 150;
            }
        }
    }
}
