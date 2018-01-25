﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_ItemScript : MonoBehaviour {

    public GameObject rngMaster;
    public GameObject KaguyaChan;
    public GameObject correctIndicator;
    public GameObject wrongIndicator;
    public bool isMoving = false;
    public bool isCorrect = false;
    public float initialScale;

    private Vector3 startingPosition;
    private bool isSelectable = false;
    private Rigidbody2D rb2d;
    private Quaternion defaultRotation;
    private bool isFinished = false;
    private float appearDelay = 2.3f;

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
        if (rngMaster.gameObject.GetComponent<KaguyaMemory_RNGDeciderScript>() != null)
        {
            appearDelay = rngMaster.gameObject.GetComponent<KaguyaMemory_RNGDeciderScript>().showDelay + 1.3f;
        }

        defaultRotation = transform.rotation;
        GetComponent<Rigidbody2D>().gravityScale = 0;
        initialScale = transform.localScale.x;

        Invoke("obtainStartingPosition", 0.01f);
        Invoke("appearSelectable", appearDelay);
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
                panStereo: AudioHelper.getAudioPan(0));
            }
            else
            {
                theIndicator = Instantiate(wrongIndicator);
                MicrogameController.instance.setVictory(false, true);
                KaguyaChan.GetComponent<KaguyaMemory_KaguyaEndAnimation>().isLose = true;
                MicrogameController.instance.playSFX(wrongSound, volume: 0.5f,
                panStereo: AudioHelper.getAudioPan(0));
            }
            theIndicator.transform.position = transform.position;
            rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished = true;
            isFinished = true;
            isSelectable = false;
        }
        
    }

    void Update()
    {
        if (isSelectable && rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished == false)
        {
            float scale = (1f + (Mathf.Sin(Time.time * 8f) / 5f)) * initialScale;
            transform.localScale = new Vector3(scale, scale, 1f);
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

        transform.rotation = defaultRotation;
        rb2d.angularVelocity = 0;
        transform.position = startingPosition;
        GetComponent<SpriteRenderer>().enabled = true;
        rb2d.velocity = new Vector2(0, 0);

        GetComponent<Rigidbody2D>().gravityScale = 0;
        isSelectable = true;
    }

    void obtainStartingPosition()
    {
        startingPosition = transform.position;
    }
}
