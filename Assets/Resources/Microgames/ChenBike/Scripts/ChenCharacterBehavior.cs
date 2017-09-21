using UnityEngine;
using System.Collections;

public class ChenCharacterBehavior : MonoBehaviour {

    public float speed = 1f;
    public float divider = 4f;
    public bool honkedat = false;
    public bool enableold = false;

    public Sprite honkedsprite;
    public Sprite ripsprite;
    private Vector3 newPosition;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    public GameObject questionm;
    public GameObject charshadow;
    public Animator charAnimator;
    public bool isren;
    public bool istewi;
    public bool isreisen;
    public bool isnazrin;
    public bool iskyouko;
    public bool issuika;
    public bool hasShadow;
    public string bushjumpanim;
    public ChenBikePlayerFail ifdead;
    public ChenBikePlayerFail ifdead2;
    public ChenBikePlayerFail ifdead3;
    public int sortingorder;

    // Use this for initialization
    void Start(){
        newPosition = transform.parent.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        charAnimator = GetComponent<Animator>();
        charAnimator.SetBool("isRen", isren);
        charAnimator.SetBool("isTewi", istewi);
        charAnimator.SetBool("isReisen", isreisen);
        charAnimator.SetBool("isNazrin", isnazrin);
        charAnimator.SetBool("isKyouko", iskyouko);
        charAnimator.SetBool("isSuika", issuika);
    }
	
	// Update is called once per frame
	void Update(){
        if (honkedat == false)
        {
            newPosition.x += Time.deltaTime * speed;
            newPosition.y -= Time.deltaTime * speed / divider;
            transform.parent.position = newPosition;
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
        if (enableold == false)
        {
            if (ChenBikePlayer.honking == true && other.name == "ChenHonk")
            {
                honkedat = true;
                Destroy(questionm);
                charAnimator.Play(bushjumpanim);
                if (hasShadow == true)
                {
                    Destroy(charshadow);
                }
            }

            if (honkedat == false && other.name == "ChenBody")
            {
                honkedat = true;
                Destroy(questionm);
                charAnimator.enabled = false;
                spriteRenderer.sprite = ripsprite;
                ifdead.dead = true;
                ifdead2.dead = true;
                ifdead3.dead = true;
            }
        }else
        {
            if (ChenBikePlayer.honking == true && other.name == "ChenHonk")
            {
                honkedat = true;
                Destroy(questionm);
                charAnimator.Play("Bang");
            }

            if (honkedat == false && other.name == "ChenBody")
            {
                honkedat = true;
                Destroy(questionm);
                charAnimator.Play("Bang");
                ifdead.dead = true;
                ifdead2.dead = true;
                ifdead3.dead = true;
            }
        }
    }
}
