using UnityEngine;
using System.Collections;

public class ChenCharacterBehavior : MonoBehaviour {

    public float speed = 1f;
    public float divider = 4f;
    public bool honkedat = false;

    public Sprite placeholderhonkedsprite;
    private Vector3 newPosition;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    public GameObject questionm;
    public Animator charAnimator;
    public ChenBikePlayerFail ifdead;
    public ChenBikePlayerFail ifdead2;

    // Use this for initialization
    void Start () {
        newPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        charAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!honkedat)
        {
             newPosition += new Vector3(Time.deltaTime * speed, -Time.deltaTime * speed/divider, 0f);
            transform.position = newPosition;
        } else
        {
            charAnimator.Play("Bang");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        collide(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        collide(other);
    }

    void collide(Collider2D other)
    {
        if ((ChenBikePlayer.honking && other.name.Contains("ChenHonk")) || (!honkedat && other.name.Contains("ChenBody"))) //this is something i do a lot w/ unity b/c sometimes the name isn't exact
        {
            honkedat = true;
            spriteRenderer.sprite = placeholderhonkedsprite; //placeholder
            Destroy(questionm);
	if (!honkedat && other.name.Contains("ChenBody"))
        {
           
            ifdead.dead = true;
            ifdead2.dead = true;
        }
        }
    }
}
