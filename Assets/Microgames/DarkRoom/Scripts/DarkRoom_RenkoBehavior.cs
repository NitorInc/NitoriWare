
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_RenkoBehavior : MonoBehaviour {

    [Header("Adjustables")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float fallAccel;
    [SerializeField] private float trapdoorFailRange;
    [SerializeField] private float lightScaleSpeed;

    private bool hasFailed = false;

    private bool isFalling = false;
    private float fallSpeed = 5f;

    // Miscellaneous

    private Transform transformRenkoLight;

    /* Base methods */

    void Start () {
        // Initialization
        transformRenkoLight = transform.Find("LightMask");
    }

	void Update () {

        // Handle movement
        if (isFalling)
            Fall();
        else
        if (!isFalling)
            Walk();

        // Handle fail state
        HandleFailure();

        if (MicrogameController.instance.isDebugMode() && Input.GetKeyDown(KeyCode.S))
            Fail();

	}

    /* My methods */

    private void Walk() {
        // Walk forward
        transform.position += new Vector3(walkSpeed, 0f, 0f) * Time.deltaTime * DarkRoomEffectAnimationController.instance.walkSpeed;
    }

    private void Fall() {
        if (transform.position.y < -5f)
            return;

        // Fall down
        fallSpeed += fallAccel * Time.deltaTime;
        transform.position += new Vector3(0f, -fallSpeed * Time.deltaTime, 0f);
    }

    private void Fail() {
        hasFailed = true;

        // Remove camera as child game object
        //Transform transformCamera = MainCameraSingleton.instance.transform;
        //if (transformCamera != null)
        //    transformCamera.parent = null;

        // Set fail sprite, animation, etc.
        Animator animatorBody = gameObject.GetComponentInChildren<Animator>();
        animatorBody.SetTrigger("Death");
    }

    private void HandleFailure() {
        if (!hasFailed) return;

        // Enlarge Renko's light mask
        //transformRenkoLight.localScale += new Vector3(1f, 1f, 1f) * lightScaleSpeed * Time.deltaTime;

    }

    /* Collision handling */

    private void OnTriggerStay2D(Collider2D otherCollider) {
        GameObject other = otherCollider.gameObject;

        // WITH: Trapdoor
        if (other.name.Length >= 8 && other.name == "Trapdoor") {
            DarkRoom_TrapdoorBehavior scriptTrapdoor = other.GetComponent<DarkRoom_TrapdoorBehavior>();
            float distanceFromTrapdoor = Mathf.Abs(transform.position.x - other.transform.position.x);

            if (distanceFromTrapdoor < trapdoorFailRange && scriptTrapdoor.CloseTimer == 0f) {
                isFalling = true;
                Fail();
            }
        } else
        // WITH: Bat
        if (other.name.Length >= 3 && other.name.Substring(0, 3) == "Bat") {
            DarkRoom_BatBehavior scriptBat = other.GetComponent<DarkRoom_BatBehavior>();
            if (!scriptBat.HasFlownAway)
                Fail();
        } else
        // WITH: Spider
        if (other.name.Length >= 4 && other.name.Substring(0, 4) == "Head") {
            Fail();
        }
    }

}
