using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_BlackBarScript : MonoBehaviour {

    [SerializeField]
    private AudioClip clapSound;

    public int myDirection = 1;                 //right is 1 and left is -1
    public float stopPoint = 0;
    private int timer1 = 0;
    private int timer2 = 0;
    public float movementSpeed = 4.0f;
    private Rigidbody2D rb2d;
    private bool exiting = false;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();

        //move based on direction given
        movementSpeed *= myDirection;
        rb2d.velocity = new Vector2(movementSpeed, 0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (myDirection == 1 && transform.position.x > -stopPoint && timer2 == 0)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            timer2 = 1;
        }
        if (myDirection == -1 && transform.position.x < stopPoint && timer2 == 0)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            timer2 = 1;
        }
        if (timer2 > 0)
        {
            timer2++;
        }
        if (timer2 > 100)
        {
            rb2d.velocity = new Vector2(movementSpeed, 0f);
            timer2 = -1;
        }



        if(myDirection == 1 && transform.position.x > 0 && timer1 == 0)
        {
            timer1 = 1;
            rb2d.velocity = new Vector2(0f, 0f);
            MicrogameController.instance.playSFX(clapSound, volume: 0.5f,
            panStero: AudioHelper.getAudioPan(transform.position.x));
        }
        if(myDirection == -1 && transform.position.x < 0 && timer1 == 0)
        {
            timer1 = 1;
            rb2d.velocity = new Vector2(0f, 0f);
        }

        if(timer1 > 0)
        {
            timer1++;
        }
        if(timer1 > 20)
        {
            timer1 = -1;
            rb2d.velocity = new Vector2(-movementSpeed, 0f);
        }
        
	}
}
