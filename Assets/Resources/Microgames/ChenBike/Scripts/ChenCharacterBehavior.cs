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

    // Use this for initialization
    void Start () {
        newPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        charAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (honkedat == false)
        {
            newPosition.x += Time.deltaTime * speed;
            newPosition.y -= Time.deltaTime * speed / divider;
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
        if (ChenBikePlayer.honking == true && other.name == "ChenHonk")
        {
            honkedat = true;
            spriteRenderer.sprite = placeholderhonkedsprite; //placeholder
            Destroy(questionm);
        }

        if (honkedat == false && other.name == "ChenBody")
        {
            Destroy(gameObject);
            MicrogameController.instance.setVictory(false, true);
        }
    }
}
