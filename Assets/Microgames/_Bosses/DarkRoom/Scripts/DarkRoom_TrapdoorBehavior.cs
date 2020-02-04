﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_TrapdoorBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float rotationSpeed;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float closeDelay;
    [SerializeField] private float openingSpeedMult;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer lampOpen;
    [SerializeField] private SpriteRenderer lampClose;
    [SerializeField]
    private AudioClip openClip;
    [SerializeField]
    private AudioClip shutClip;
    [SerializeField]
    private bool reverseLogic;

    private GameObject myDoor;
    private GameObject myLamp;
    private SpriteRenderer myLampSpriteRenderer;
    private AudioSource sfxSource;

    private Quaternion targetRotation;
    private float closeTimer = 0f;

    private DarkRoomInstrumentDistance instrumentDistance;

	/* Base methods */

	void Start () {
        // Initialization
        myDoor = transform.Find("Rig/Door").gameObject;
        //myLamp = transform.Find("Rig/Hinge/Lamp").gameObject;
        instrumentDistance = GetComponent<DarkRoomInstrumentDistance>();
        sfxSource = GetComponent<AudioSource>();
	}

	void Update () {

        // Handle timer
        if (closeTimer - 60 * Time.deltaTime > 0f)
            closeTimer -= 60 * Time.deltaTime;
        else
        {
            if (closeTimer > 0f)
                sfxSource.PlayOneShot(shutClip);
            closeTimer = 0f;
        }

        var c = lampOpen.color;
        c.a = Mathf.InverseLerp(0f, closeDelay, closeTimer);
        lampOpen.color = c;
        c = lampClose.color;
        c.a = Mathf.InverseLerp(closeDelay, 0f, closeTimer);
        lampClose.color = c;

        // Handle door rotation
        HandleDoorRotation();

        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);

	}

    /* My methods */

    private void HandleDoorRotation() {
        // Handle door rotation
        myDoor.transform.localEulerAngles = new Vector3(myDoor.transform.localEulerAngles.x, myDoor.transform.localEulerAngles.y,
            Mathf.MoveTowards(myDoor.transform.localEulerAngles.z, isClosed ? 0f : 90f, rotationSpeed * Time.deltaTime));
        //myDoor.transform.rotation = Quaternion.RotateTowards(myDoor.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        instrumentDistance.enabled = !isClosed;
    }

    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            if (closeTimer == 0f)
                sfxSource.PlayOneShot(shutClip);
            closeTimer = Mathf.MoveTowards(closeTimer, closeDelay, Time.deltaTime * openingSpeedMult * 60f);
        }

    }

    /* Getters and setters */

    public bool isClosed => closeTimer > 0 != reverseLogic;

}
