using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_TrapdoorBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float rotationSpeed;
    [Header("Alarms | counts down by 1 per frame.")]
    [SerializeField] private float closeDelay;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer lampOpen;
    [SerializeField] private SpriteRenderer lampClose;

    private GameObject myDoor;
    private GameObject myLamp;
    private SpriteRenderer myLampSpriteRenderer;

    private Quaternion targetRotation;
    private float closeTimer = 0f;

    private DarkRoomInstrumentDistance instrumentDistance;

	/* Base methods */

	void Start () {
        // Initialization
        myDoor = transform.Find("Rig/Door").gameObject;
        //myLamp = transform.Find("Rig/Hinge/Lamp").gameObject;
        instrumentDistance = GetComponent<DarkRoomInstrumentDistance>();
	}

	void Update () {

        // Handle timer
        if (closeTimer - 60 * Time.deltaTime > 0f)
            closeTimer -= 60 * Time.deltaTime;
        else
            closeTimer = 0f;

        var c = lampOpen.color;
        c.a = Mathf.InverseLerp(0f, closeDelay, closeTimer);
        lampOpen.color = c;
        c = lampClose.color;
        c.a = Mathf.InverseLerp(closeDelay, 0f, closeTimer);
        lampClose.color = c;

        // Handle door rotation
        HandleDoorRotation();

	}

    /* My methods */

    private void HandleDoorRotation() {
        // Handle door rotation
        myDoor.transform.rotation = Quaternion.RotateTowards(myDoor.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        targetRotation = (closeTimer <= 0f) ? Quaternion.Euler(0f, 0f, 90f) : Quaternion.Euler(0f, 0f, 0f);
        instrumentDistance.enabled = CloseTimer <= 0f;
    }

    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Light
        if (other.name == "Light") {
            closeTimer = closeDelay;
        }

    }

    /* Getters and setters */

    public float CloseTimer { get { return closeTimer; } set { closeTimer = value; } }

}
