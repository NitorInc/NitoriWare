using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareKogasaBehaviour : MonoBehaviour
{

    public Animator kogasaAnimator;
    public SpriteRenderer kogasaSpriteRenderer;
    public bool iswalking = false;
    public string kogasawalkanim;
    public string kogasawalkanimreverse;
    public Sprite stillSprite;
    public float moveSpeed;
    public float maxposXleft;
    public float maxposXright;

    private int direction;

    // Use this for initialization
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            //TODO scare
        }
        else
            updateMovement();


    }

    void updateMovement()
    {
        bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightPressed = Input.GetKey(KeyCode.RightArrow);

        if (!(leftPressed && rightPressed))
        {
            if (kogasaSpriteRenderer.sprite == stillSprite)
            {
                direction = leftPressed ? -1 : (rightPressed ? 1 : 0);
            }
            else
            {
                if (direction == -1 && rightPressed)
                    direction = 1;
                else if (direction == 1 && leftPressed)
                    direction = -1;
            }
        }

        kogasaAnimator.speed = (leftPressed || rightPressed) ? 1f : 1.5f;
        transform.position += Vector3.right * (float)direction * moveSpeed * Time.deltaTime; // * ((leftPressed || rightPressed) ? 1f : .5f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, maxposXleft, maxposXright), transform.position.y, transform.position.z);

        kogasaAnimator.SetInteger("direction", direction);
    }

    //bool isOnMovementSprite()
    //{
    //    foreach (Sprite sprite in kogasaMovementSprites)
    //    {
    //        if (spriteRenderer.sprite == sprite)
    //            return true;
    //    }
    //    return false;
    //}
}
