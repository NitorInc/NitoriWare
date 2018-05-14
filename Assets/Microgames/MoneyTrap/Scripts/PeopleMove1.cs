using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleMove1 : MonoBehaviour {

    //Controls movement for people objects

    [Header("How fast person moves")]
    [SerializeField]
    private float speed = 1f;

    [Header("Proximity threshold to follow")]
    [SerializeField]
    private float proximity = 1f;

    //if this person is trapped
    private bool trapped;
    // Stores the direction of movement
    private Vector2 trajectory;

    // Use this for initialization
    void Start () {
        //the person starts free
        trapped = false;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //if collides with trap
        if (other.gameObject.name == "Trap")
        {
            trapped = true;
            trajectory.y = -10f;
            //gameObject.SetActive(false);
        }
        // Test that this works
        print("People hit:" + other.gameObject.name);
    }

    // Update is called once per frame
    void Update ()
    { 
        //if this person is still free
        if (!trapped)
        {
            //get player reference
            GameObject player = GameObject.Find("Player");

            //get the direction towards player from this object's position
            trajectory = (player.transform.position - transform.position).normalized;
            //nullify y axis for horizontal(x) movement only
            trajectory.y = 0f;

            // if player is within proximity threshold
            if (Mathf.Abs(transform.position.x - player.transform.position.x) < proximity)
            {
                //move towards player's x position at defined speed
                Vector2 newPosition = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
                transform.position = newPosition;
            }
        }
        //if this person was trapped still hasn't burned in touhou hell
        else if (gameObject.activeSelf)
        {
            //if not yet in touhou hell
            if (transform.position.y > -7f)
            {
                //move downwards into touhou hell
                Vector2 newPosition = (Vector2)transform.position + (Vector2.Lerp(transform.position, trajectory, 0.5f) * Time.deltaTime);
                transform.position = newPosition;
            }
            else
            {
                //Burn to ashes >:D
                GameObject.Destroy(gameObject);
            }
        }
    }
}
