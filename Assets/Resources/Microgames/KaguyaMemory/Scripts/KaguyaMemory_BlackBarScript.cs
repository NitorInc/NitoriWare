using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_BlackBarScript : MonoBehaviour {

    public int myDirection = 1;                 //right is 1 and left is -1
    public float stopPoint = 0;
    public float movementSpeed = 4.0f;

    private int phase = 0;
    private bool exiting = false;
    private Rigidbody2D rb2d;

    [SerializeField]
    private AudioClip clapSound;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();

        //move based on direction given
        movementSpeed *= myDirection;
        rb2d.velocity = new Vector2(movementSpeed, 0f);
    }
	
    void closeBars()
    {
        rb2d.velocity = new Vector2(movementSpeed, 0f);
    }
    void openBars()
    {
        rb2d.velocity = new Vector2(-movementSpeed, 0f);
    }

    // Update is called once per frame
    void Update () {
        //Stop where the unity editor says
        if (myDirection == 1 && transform.position.x > -stopPoint && phase == 0)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 1;
            Invoke("closeBars", 1.6f);
        }
        if (myDirection == -1 && transform.position.x < stopPoint && phase == 0)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 1;
            Invoke("closeBars", 1.6f);
        }

        //Stay shut shortly when both bars meet the center of the screen
        if(myDirection == 1 && transform.position.x > 0 && phase == 1)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            MicrogameController.instance.playSFX(clapSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));
            Invoke("openBars", 0.3f);
        }
        if(myDirection == -1 && transform.position.x < 0 && phase == 1)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            Invoke("openBars", 0.3f);
        }
	}
}
