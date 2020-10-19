
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoom_RenkoBehavior : MonoBehaviour {

    public static DarkRoom_RenkoBehavior instance;

    [SerializeField]
    private Collider2D lightCollider;

    [Header("Adjustables")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float fallAccel;
    [SerializeField] private float trapdoorFailRange;
    [SerializeField] private float lightScaleSpeed;
    [SerializeField]
    private float commandTime = 1f;

    private bool hasFailed = false;

    private bool isFalling = false;
    private float fallSpeed = 5f;

    // Miscellaneous

    private Transform transformRenkoLight;

    /* Base methods */

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        // Initialization
        transformRenkoLight = transform.Find("LightMask");

        Invoke("command", commandTime);
    }

    void command()
    {
        MicrogameController.instance.displayLocalizedCommand("commandb", "Protect!");
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

        if (MicrogameController.instance.isDebugMode() && Input.GetKeyDown(KeyCode.D))
            Fail();


        if (MicrogameController.instance.isDebugMode() && Input.GetKeyDown(KeyCode.S))
            Time.timeScale *= 4f;
        if (MicrogameController.instance.isDebugMode() && Input.GetKeyUp(KeyCode.S))
            Time.timeScale /= 4f;


    }

    /* My methods */

    private void Walk() {
        // Walk forward
        transform.position += new Vector3(walkSpeed, 0f, 0f) * Time.deltaTime * DarkRoomEffectAnimationController.instance.walkSpeed;
    }

    private void Fall() {
        if (transform.position.y < -7.2f)
            return;

        // Fall down
        fallSpeed += fallAccel * Time.deltaTime;
        transform.position += new Vector3(0f, -fallSpeed * Time.deltaTime, 0f);
    }

    public void Fail() {
        if (hasFailed)
            return;
        hasFailed = true;

        // Remove camera as child game object
        //Transform transformCamera = MainCameraSingleton.instance.transform;
        //if (transformCamera != null)
        //    transformCamera.parent = null;

        // Set fail sprite, animation, etc.
        Animator animatorBody = gameObject.GetComponentInChildren<Animator>();
        MicrogameController.instance.setVictory(false);
        animatorBody.SetTrigger("Death");
        lightCollider.offset = new Vector2(1000f, 0f);
    }

    public void Win()
    {
        lightCollider.offset = new Vector2(1000f, 0f);
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
        if (other.name.Contains("Pit")) {
            DarkRoom_TrapdoorBehavior scriptTrapdoor = other.GetComponentInParent<DarkRoom_TrapdoorBehavior>();
            float distanceFromTrapdoor = Mathf.Abs(transform.position.x - other.transform.position.x);

            if (distanceFromTrapdoor < trapdoorFailRange && !scriptTrapdoor.isClosed) {
                isFalling = true;
                MainCameraSingleton.instance.transform.parent = null;
                Fail();
            }
        } else
        // WITH: Bat
        if (other.name.Length >= 3 && other.name.Substring(0, 3) == "Bat") {
            DarkRoom_BatBehavior scriptBat = other.GetComponent<DarkRoom_BatBehavior>();
            if (!scriptBat.HasFlownAway)
            {
                scriptBat.killedPlayer = true;
                Fail();
            }
        } else
        // WITH: Spider
        if (other.name.Length >= 4 && other.name.Substring(0, 4) == "Head") {
            {

                DarkRoom_SpiderHeadBehavior scriptSpider = other.GetComponent<DarkRoom_SpiderHeadBehavior>();
                scriptSpider.killedPlayer = true;
                Fail();
            }
        }
    }

}
