using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_ItemScript : MonoBehaviour {

    public GameObject rngMaster;
    public GameObject KaguyaChan;
    public GameObject correctIndicator;
    public GameObject wrongIndicator;
    public bool isMoving = false;
    public bool isCorrect = false;
    public float movementSpeed = 0;

    private Vector3 startingPosition;
    private bool isSelectable = false;
    private Rigidbody2D rb2d;
    private Quaternion defaultRotation;
    private bool isFinished = false;

    [SerializeField]
    private AudioClip correctSound;

    [SerializeField]
    private AudioClip wrongSound;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().enabled = false;
        
        rb2d = GetComponent<Rigidbody2D>();

        if (GetComponent<CapsuleCollider2D> () != null)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        if (GetComponent<CircleCollider2D>() != null)
        {
            GetComponent<CircleCollider2D>().enabled = false;
        }

        defaultRotation = transform.rotation;
        GetComponent<Rigidbody2D>().gravityScale = 0;

        Invoke("obtainStartingPosition", 0.5f);
        Invoke("appearSelectable", 2.3f);
    }

    void OnMouseDown()
    {
        if(rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished == false && isSelectable == true)
        {
            GameObject theIndicator;
            if (isCorrect == true)
            {
                theIndicator = Instantiate(correctIndicator);
                MicrogameController.instance.setVictory(true, true);
                KaguyaChan.GetComponent<KaguyaMemory_KaguyaEndAnimation>().isWin = true;
                MicrogameController.instance.playSFX(correctSound, volume: 0.5f,
                panStero: AudioHelper.getAudioPan(0));
            }
            else
            {
                theIndicator = Instantiate(wrongIndicator);
                MicrogameController.instance.setVictory(false, true);
                KaguyaChan.GetComponent<KaguyaMemory_KaguyaEndAnimation>().isLose = true;
                MicrogameController.instance.playSFX(wrongSound, volume: 0.5f,
                panStero: AudioHelper.getAudioPan(0));
            }
            theIndicator.transform.position = transform.position;
            rb2d.velocity = new Vector2(0, 0);
            rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished = true;
            isFinished = true;
            isSelectable = false;
        }
        
    }

    void Update()
    {
        transform.rotation = defaultRotation;
        rb2d.angularVelocity = 0;
        if (isFinished == true)
        {
            rb2d.velocity = new Vector2(0, 0);
            return;
        }
        if(rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished == true)
        {
            isFinished = true;
            rb2d.velocity = new Vector2(0, 0);
            return;
        }
        //prevent objects from moving out of bounds
        if (transform.position.x > 5f)
        {
            rb2d.velocity = new Vector2(-movementSpeed, rb2d.velocity.y);
        }
        if (transform.position.x < -5.8f)
        {
            rb2d.velocity = new Vector2(movementSpeed, rb2d.velocity.y);
        }
        if (transform.position.y > 3.5f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -movementSpeed);
        }
        if (transform.position.y < -3.8f)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, movementSpeed);
        }
        
        //fix speed after bounces
        if (rb2d.velocity.x > 0 && rb2d.velocity.x < movementSpeed)
        {
            rb2d.velocity = new Vector2(movementSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.x < 0 && rb2d.velocity.x > -movementSpeed)
        {
            rb2d.velocity = new Vector2(-movementSpeed, rb2d.velocity.y);
        }
        if (rb2d.velocity.y > 0 && rb2d.velocity.y < movementSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, movementSpeed);
        }
        if (rb2d.velocity.y < 0 && rb2d.velocity.y > -movementSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -movementSpeed);
        }
    }
    void appearSelectable()
    {
        if (GetComponent<CapsuleCollider2D>() != null)
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
        if (GetComponent<CircleCollider2D>() != null)
        {
            GetComponent<CircleCollider2D>().enabled = true;
        }

        transform.position = startingPosition;
        GetComponent<SpriteRenderer>().enabled = true;

        float vSpeed = movementSpeed;
        float hSpeed = movementSpeed;
        int randomNumber = Random.Range(0, 100);

        if(randomNumber < 50)
        {
            hSpeed *= -1;
        }

        randomNumber = Random.Range(0, 100);

        if (randomNumber < 50)
        {
            vSpeed *= -1;
        }

        rb2d.velocity = new Vector2(hSpeed, vSpeed);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        isSelectable = true;
    }

    void obtainStartingPosition()
    {
        startingPosition = transform.position;
    }
}
