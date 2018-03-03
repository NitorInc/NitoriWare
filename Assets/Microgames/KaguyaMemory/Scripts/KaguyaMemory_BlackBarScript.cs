using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_BlackBarScript : MonoBehaviour {

    public int myDirection = 1;                 //right is 1 and left is -1
    public float stopPoint = 0;
    public float initialOpenSpeed = 2.0f;
    public float movementSpeed = 4.0f;
    public GameObject RNGMaster;

    private int phase = 0;
    private bool exiting = false;
    private Rigidbody2D rb2d;
    private float currentSpeed;

    [SerializeField]
    private float closeDelay = 1.6f;

    [SerializeField]
    private float openDelay = 0.3f;

    [SerializeField]
    private AudioClip clapSound;

    

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();

        //move based on direction given
        movementSpeed *= myDirection;
        initialOpenSpeed *= myDirection;
        currentSpeed = initialOpenSpeed;
        Invoke("openBars", 0.4f);

        if (RNGMaster.gameObject.GetComponent<KaguyaMemory_RNGDeciderScript>() != null)
        {
            closeDelay = RNGMaster.gameObject.GetComponent<KaguyaMemory_RNGDeciderScript>().showDelay + 0.6f;
        }
    }
	
    void closeBars()
    {
        currentSpeed = movementSpeed;
        rb2d.velocity = new Vector2(currentSpeed, 0f);
    }
    void openBars()
    {
        rb2d.velocity = new Vector2(-currentSpeed, 0f);
    }

    // Update is called once per frame
    void FixedUpdate () {
        //Stop where the unity editor says
        if (myDirection == 1 && transform.position.x < -stopPoint && phase == 0)
        {
            
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 1;
            Invoke("closeBars", closeDelay);
        }
        if (myDirection == -1 && transform.position.x > stopPoint && phase == 0)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 1;
            Invoke("closeBars", closeDelay);
        }

        //Stay shut shortly when both bars meet the center of the screen
        if(myDirection == 1 && transform.position.x > -0.25 && phase == 1)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            MicrogameController.instance.playSFX(clapSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));
            Invoke("openBars", openDelay);
        }
        if(myDirection == -1 && transform.position.x < 0.13 && phase == 1)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            Invoke("openBars", openDelay);
        }
	}
}
