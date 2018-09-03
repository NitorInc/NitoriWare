using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareKogasaBehaviour : MonoBehaviour
{

    public Animator kogasaAnimator;
    public SpriteRenderer kogasaSpriteRenderer;
    public KogasaScareVictimBehavior victim;
    public string kogasawalkanim;
    public string kogasawalkanimreverse;
    public Sprite stillSprite;
    public float moveSpeed;
    public float screenShakeFailureMult = .5f;

    [Header("Spawn constraints")]
    public float minSpawnX;
    public float maxSpawnX;

    [Header("Walk constraints")]
    public float maxposXleft;
    public float maxposXright;

    [Header("Auto-snaps to an X disance away from the victim afte scaring")]
    public float minScareDistance;
    public float scareShiftSpeed;

    [Header("Looping sound for walking")]
    public AudioSource walkSource;

    private bool victimInSight;
    private int direction;
    private State state;
    public enum State
    {
        Default,
        Victory,
        Loss
    }

    private void Awake()
    {
        victimInSight = false;
        state = State.Default;

        transform.position = new Vector3(Random.Range(minSpawnX, maxSpawnX), transform.position.y, transform.position.z);
    }

    void Update()
    {
        switch (state)
        {
            case (State.Default):
                //Handle scare
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Reset animator speed
                    kogasaAnimator.speed = 1f;

                    //Stop step sound
                    walkSource.loop = false;

                    //Handle victory/loss
                    if (victimInSight)
                        victory();
                    else
                        loss();

                    //Make sure player faces victim
                    if (victim.transform.position.x > transform.position.x)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                    //Set animation triggers
                    kogasaAnimator.SetTrigger("scare");
                    kogasaAnimator.SetInteger("state", (int)state);
                    kogasaAnimator.SetTrigger("arrowsOff");
                }
                else
                    updateMovement();
                break;
            case (State.Victory):
            case (State.Loss):
                if (Mathf.Abs(transform.position.x - victim.transform.position.x) < minScareDistance)
                {
                    float snapDirection = Mathf.Sign(transform.position.x - victim.transform.position.x);
                    //transform.moveTowards2D((Vector2)victimInSight.transform.position + (Vector2.right * snapDirection * minScareDistance), scareShiftSpeed);
                    transform.position += Vector3.right * snapDirection * scareShiftSpeed * Time.deltaTime;
                }
                break;
            default:
                break;
        }


    }

    void victory()
    {
        MicrogameController.instance.setVictory(true, true);
        state = State.Victory;

        victim.scare(true, (int)Mathf.Sign(transform.position.x - victim.transform.position.x));
        //Destroy(victimInSight.gameObject);
    }

    void loss()
    {
        MicrogameController.instance.setVictory(false, true);
        state = State.Loss;

        victim.scare(false, (int)Mathf.Sign(transform.position.x - victim.transform.position.x));
    }

    public void shakeScreen(float shakeAmount)
    {
        if (state == State.Loss)
        {
            shakeAmount *= screenShakeFailureMult;
            //CameraShake.instance.shakeCoolRate *= screenShakeFailureMult;
            CameraShake.instance.shakeSpeed *= screenShakeFailureMult;
        }
        CameraShake.instance.setScreenShake(shakeAmount);
    }

    void updateMovement()
    {
        bool leftPressed = Input.GetKey(KeyCode.LeftArrow);
        bool rightPressed = Input.GetKey(KeyCode.RightArrow);

        if (leftPressed || rightPressed)
            kogasaAnimator.SetTrigger("arrowsOff");

        if (!(leftPressed && rightPressed))
        {
            //Only allow transitions to idle from one particular sprite, that way the animation isn't janky
            if (kogasaSpriteRenderer.sprite == stillSprite)
            {
                //If we're on the "still sprite", any of the three directions are allowed
                direction = leftPressed ? -1 : (rightPressed ? 1 : 0);
            }
            else
            {
                //We can transition from left to right movement (or vice-versa) without having to go to idle
                if (direction == -1 && rightPressed)
                    direction = 1;
                else if (direction == 1 && leftPressed)
                    direction = -1;
            }
        }

        //Loop walkSource if walk animation is playing, don't if it's not
        if (direction != 0)
        {
            if (!walkSource.isPlaying)
                walkSource.Play();
            walkSource.loop = true;
        }
        else
        {
            walkSource.loop = false;
        }
        walkSource.panStereo = AudioHelper.getAudioPan(transform.position.x);

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
        if (state == State.Default && !victimInSight && other.name.ToLower().Contains("victim"))
            victimInSight = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (state == State.Default && victimInSight && other.name.ToLower().Contains("victim"))
            victimInSight = false;
    }
}
