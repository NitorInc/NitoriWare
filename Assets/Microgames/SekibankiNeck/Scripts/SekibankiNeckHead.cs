using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekibankiNeckHead : MonoBehaviour
{




  
    [SerializeField]
    float turningSpeed = 4f;
    [SerializeField]
    float maxTurningSpeed = 4f;
   
    private bool lastInputRight = false;

    private bool alive = true;
    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private float delay = 0.1f;
    [SerializeField]
    private Animator victoryanimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            velocity = Vector2.zero;

            if (collision.gameObject.tag == "MicrogameTag1")
            {
                if (alive)
                {

                    Kill();
                }
            }
            if (collision.gameObject.tag == "MicrogameTag2")
            {
                MicrogameController.instance.setVictory(true, true);

            Animator victoryanimator = GetComponentInChildren<Animator>();
            victoryanimator.Play("SekibankiNeckVictoryHead");
            


            SekibankiNeckHead sekibankiNeckHead = GetComponent<SekibankiNeckHead>();
                sekibankiNeckHead.enabled = false;
            }

        
        
    }

    void Kill()
    {
        alive = false;

       

        MicrogameController.instance.playSFX(deathSound, volume: 0.2f, panStereo: AudioHelper.getAudioPan(transform.position.x));

        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();

        Invoke("RemoveSprite", delay);

    }

    void RemoveSprite()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);

        SekibankiNeckHead sekibankiNeckHead = GetComponent<SekibankiNeckHead>();
        sekibankiNeckHead.enabled = false;

        MicrogameController.instance.setVictory(victory: false, final: true);
    }


    Vector2 velocity;



    void Update()
    {

        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                velocity.y += -turningSpeed * Time.deltaTime;
                clampSpeed();
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                velocity.y += turningSpeed * Time.deltaTime;
                clampSpeed();
            }
            else
            {
                if (velocity.y < -0.5f)
                {
                    velocity.y -= -turningSpeed * Time.deltaTime;
                    clampSpeed();
                }
                else if (velocity.y > 0.5f)
                {
                    velocity.y -= turningSpeed * Time.deltaTime;
                    clampSpeed();
                }
                else velocity.y = 0;
            }


            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    velocity.x += -turningSpeed * Time.deltaTime;
                    clampSpeed();
                    if (lastInputRight != true)
                    {

                        transform.Rotate(new Vector3(0, 180, 0));
                        lastInputRight = true;
                    }

                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    velocity.x += turningSpeed * Time.deltaTime;
                    clampSpeed();
                    if (lastInputRight != false)
                    {

                        transform.Rotate(new Vector3(0, 180, 0));
                        lastInputRight = false;
                    }

                }
                else
                {
                    if (velocity.x < -0.5f)
                    {
                        velocity.x -= -turningSpeed * Time.deltaTime;
                        clampSpeed();
                    }
                    else if (velocity.x > 0.5f)
                    {
                        velocity.x -= turningSpeed * Time.deltaTime;
                        clampSpeed();
                    }
                    else velocity.x = 0;
                }
            }

            transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0);
        }




    }

   

        void clampSpeed()
        {
            if (velocity.y > maxTurningSpeed)
                velocity.y = maxTurningSpeed;
            if (velocity.y < -maxTurningSpeed)
                velocity.y = -maxTurningSpeed;
            if (velocity.x > maxTurningSpeed)
                velocity.x = maxTurningSpeed;
            if (velocity.x < -maxTurningSpeed)
                velocity.x = -maxTurningSpeed;
        }



    } 
