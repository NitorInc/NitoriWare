using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_BlackBarScript : MonoBehaviour {

    public int myDirection = 1;                 //right is 1 and left is -1
    public float stopPoint = 0;
    public float openSpeed = 4f;
    public float closeSpeed = 12f;
    public float closeAfterItemThrowDelay = .4f;
    public float doorsClosedTime = .3f;
    public GameObject RNGMaster;
    public Transform otherDoor;

    private int phase = 0;
    private bool exiting = false;
    private Rigidbody2D rb2d;
    private float closeDelay = 1.6f;

    [SerializeField]
    private AudioClip clapSound;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();

        //move based on direction given
        //movementSpeed *= myDirection;
        Invoke("openBars", 0.4f);

        if (RNGMaster.gameObject.GetComponent<KaguyaMemory_RNGDeciderScript>() != null)
        {
            closeDelay = RNGMaster.gameObject.GetComponent<KaguyaMemory_RNGDeciderScript>().showDelay + closeAfterItemThrowDelay;
        }
    }
	
    void closeBars()
    {
        rb2d.velocity = new Vector2(closeSpeed * myDirection, 0f);
    }
    void openBars()
    {
        rb2d.velocity = new Vector2(-openSpeed * myDirection, 0f);
    }

    // Update is called once per frame
    void Update () {
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
        if(myDirection == 1 && transform.position.x >= -0.13f && phase == 1)
        {
            transform.position = new Vector3(-.13f, transform.position.y, transform.position.z);
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            MicrogameController.instance.playSFX(clapSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));
            Invoke("openBars", doorsClosedTime);
        }
        if(myDirection == -1 && transform.position.x <= 0.13f && phase == 1)
        {
            transform.position = new Vector3(.13f, transform.position.y, transform.position.z);
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            Invoke("openBars", doorsClosedTime);
        }

        otherDoor.transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
	}
}
