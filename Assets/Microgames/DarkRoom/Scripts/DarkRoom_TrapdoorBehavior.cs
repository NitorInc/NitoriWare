using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_TrapdoorBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float rotationSpeed;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float closeDelay;

    [Header("Sprites")]
    [SerializeField] private Sprite lampOpen;
    [SerializeField] private Sprite lampClose;

    private GameObject myDoor;
    private GameObject myLamp;
    private SpriteRenderer myLampSpriteRenderer;

    private Quaternion targetRotation;
    private float closeTimer = 0f;

	/* Base methods */

	void Start () {
        // Initialization
        myDoor = transform.Find("Rig/Door").gameObject;
        myLamp = transform.Find("Rig/Hinge/Lamp").gameObject;
	}

	void Update () {

        // Handle timer
        if (closeTimer - 60 * Time.deltaTime > 0f)
            closeTimer -= 60 * Time.deltaTime;
        else {
            myLamp.GetComponent<SpriteRenderer>().sprite = lampOpen;
            closeTimer = 0f;
        }

        // Handle door rotation
        HandleDoorRotation();

	}

    /* My methods */

    private void HandleDoorRotation() {
        // Handle door rotation
        myDoor.transform.rotation = Quaternion.RotateTowards(myDoor.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        targetRotation = (closeTimer == 0f) ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.Euler(0f, 0f, 0f);
    }

    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            myLamp.GetComponent<SpriteRenderer>().sprite = lampClose;
            closeTimer = closeDelay;
        }

    }

    /* Getters and setters */

    public float CloseTimer { get { return closeTimer; } set { closeTimer = value; } }

}
