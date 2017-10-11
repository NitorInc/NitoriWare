using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareKogasaBehaviour : MonoBehaviour
{

    public Animator kogasaAnimator;
    public bool iswalking = false;
    public static bool isscaring = false;
    public string kogasawalkanim;
    public string kogasawalkanimreverse;
    public Transform kogasaTransform;
    public Sprite[] kogasaMovementSprites;
    public float moveSpeed;
    public float movesped;
    public float maxposXleft;
    public float maxposXright;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        kogasaAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //maxposXleft = -5.214749; //set in inspector
        //maxposXright = 6.21187;  //set in inspector
    }

    // Update is called once per frame
    void Update()
    {

        isscaring = Input.GetKeyDown("space");

        if (Input.GetKey("left") && !Input.GetKey("right"))
        {
            if (kogasaTransform.position.x >= maxposXleft)
            {
                kogasaAnimator.Play(kogasawalkanim);
                //kogasaAnimator.SetFloat("Direction", 1.0f);
                if (isOnMovementSprite())
                {
                    kogasaTransform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                }
            }
        }
        if (Input.GetKeyUp("left"))
        {
            kogasaAnimator.Play("idle");
        }
        if (Input.GetKey("right") && !Input.GetKey("left"))
        {
            if (kogasaTransform.position.x <= maxposXright)
            {
                kogasaAnimator.Play(kogasawalkanimreverse);

                //kogasaAnimator.SetFloat("Direction", -1.0f); //for some reason, there's a weird stutter at the start of the animation. i'm going with two animations instead
                if (isOnMovementSprite())
                    kogasaTransform.Translate(Vector2.left * -moveSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKeyUp("right"))
        {
            kogasaAnimator.Play("idle");
        }

        if (Input.GetKey("left") && Input.GetKey("right"))
        {
            kogasaAnimator.Play("idle");
        }
    }

    bool isOnMovementSprite()
    {
        foreach (Sprite sprite in kogasaMovementSprites)
        {
            if (spriteRenderer.sprite == sprite)
                return true;
        }
        return false;
    }
}
