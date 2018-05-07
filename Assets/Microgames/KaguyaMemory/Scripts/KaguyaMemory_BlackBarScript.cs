using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_BlackBarScript : MonoBehaviour {

    public int myDirection = 1;                 //right is 1 and left is -1
    public float stopPoint = 0;
    public float openSpeed = 4f;
    public float closeSpeed = 12f;
    public GameObject RNGMaster;
    public GameObject timingMaster;
    public GameObject backgroundGraphic;
    public GameObject spotlightGraphic;
    public Transform otherDoor;

    private int phase = 0;
    private Rigidbody2D rb2d;
    private KaguyaMemory_Timing timeValues;

    [SerializeField]
    private AudioClip clapSound;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        timeValues = timingMaster.GetComponent<KaguyaMemory_Timing>();

        //move based on direction given
        Invoke("openBars", timeValues.initialDoorOpen);
        
        
    }

    void finalOpenBars()
    {
        rb2d.velocity = new Vector2(-openSpeed * myDirection, 0f);
        if (backgroundGraphic !=  null && spotlightGraphic != null)
        {
            backgroundGraphic.GetComponent<KaguyaMemory_ChangeColor>().InitiateChange();
            spotlightGraphic.GetComponent<KaguyaMemory_ChangeColor>().InitiateChange();

            for (int j = 0; j < RNGMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().maxItems; j++)
            {
                RNGMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().items[j].GetComponent<KaguyaMemory_ItemScript>().appearSelectable();
            }
        }

        MicrogameController.instance.displayLocalizedCommand("commandb", "Select!");
    }
    void closeBars()
    {
        rb2d.velocity = new Vector2(closeSpeed * myDirection, 0f);
    }
    void openBars()
    {
        rb2d.velocity = new Vector2(-openSpeed * myDirection, 0f);
    }

    void throwItem()
    {
        RNGMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().ShowItem();
    }

    // Update is called once per frame
    void Update () {
        //Stop where the unity editor says
        if (myDirection == 1 && transform.position.x < -stopPoint && phase == 0)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 1;
            Invoke("throwItem", timeValues.throwItemAfterInitialOpen);
            Invoke("closeBars", timeValues.doorCloseAfterInitialOpen);
        }
        if (myDirection == -1 && transform.position.x > stopPoint && phase == 0)
        {
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 1;
            Invoke("closeBars", timeValues.doorCloseAfterInitialOpen);
        }

        //Stay shut shortly when both bars meet the center of the screen
        if(myDirection == 1 && transform.position.x >= -0.13f && phase == 1)
        {
            transform.position = new Vector3(-.13f, transform.position.y, transform.position.z);
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            MicrogameController.instance.playSFX(clapSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));
            Invoke("finalOpenBars", timeValues.doorOpenAfterClose);
        }
        if(myDirection == -1 && transform.position.x <= 0.13f && phase == 1)
        {
            transform.position = new Vector3(.13f, transform.position.y, transform.position.z);
            rb2d.velocity = new Vector2(0f, 0f);
            phase = 2;
            Invoke("finalOpenBars", timeValues.doorOpenAfterClose);
        }

        otherDoor.transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
	}
}
