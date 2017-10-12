using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareKogasaBehaviour : MonoBehaviour
{

    public Animator kogasaAnimator;
    public SpriteRenderer kogasaSpriteRenderer;
    public KogasaScareVictimBehavior victimInSight;
    public string kogasawalkanim;
    public string kogasawalkanimreverse;
    public Sprite stillSprite;
    public float moveSpeed;
    public float maxposXleft;
    public float maxposXright;

    private int direction;

    void Start()
    {
        victimInSight = null;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("space"))
        {
            if (victimInSight == null)
                loss();
            else
                victory();

            enabled = false;
        }
        else
            updateMovement();


    }

    void victory()
    {
        MicrogameController.instance.setVictory(true, true);
        Destroy(victimInSight.gameObject);
    }
    
    void loss()
    {
        MicrogameController.instance.setVictory(false, true);
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
        if (victimInSight == null && other.name.ToLower().Contains("victim"))
            victimInSight = other.GetComponent<KogasaScareVictimBehavior>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (victimInSight != null && other.name.ToLower().Contains("victim"))
            victimInSight = null;
    }
}
