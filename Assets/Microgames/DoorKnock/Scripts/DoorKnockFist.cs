using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockFist : MonoBehaviour {
    private bool knocking = false;
    private Animator anim;

    [SerializeField]
    private float coolDown = 0.1f;
    [SerializeField]
    private GameObject door;
    [SerializeField]
    private GameObject flash;
    private float shrinkSpeed = 4f;
    [SerializeField]
    private float flashCoolDown = .25f;

    [SerializeField]
    private Transform flashRoot;
    [SerializeField]
    private Vector2 flashDisplacementXRange;
    [SerializeField]
    private Vector2 flashDisplacementYRange;

    private float timer = 0f;
    private float flashTimer = 0f;
    private bool intersecting = false;
    private bool movementDisabled;
    private ParticleSystem particleSystem;
    void Start() {
        anim = GetComponent<Animator>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update () {
        if (timer > 0) { 
                timer -= Time.deltaTime;
        } else if (MicrogameController.instance.getVictory()){
            if (!movementDisabled)
            {
                GetComponent<FollowCursor>().enabled = false;
                movementDisabled = true;
            }
            transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime * shrinkSpeed;
            if (transform.localScale.x <= 0){
                gameObject.SetActive(false);
            }
        } else if (Input.GetMouseButtonDown(0) && timer <= 0) {
            anim.SetTrigger("Knock");
            timer = coolDown;
            flashTimer = flashCoolDown;
            if (intersecting) {
                door.SendMessage("OnClick");
                particleSystem.Play();

                flash.SetActive(true);
                
                var flipResultPos = flashRoot.localPosition.x >= 0f;
                flashRoot.localPosition = new Vector3(MathHelper.randomRangeFromVector(flashDisplacementXRange),
                    MathHelper.randomRangeFromVector(flashDisplacementYRange),
                    flashRoot.localPosition.z);
                if (flipResultPos)
                {
                    flashRoot.localPosition = new Vector3(-flashRoot.localPosition.x,
                        flashRoot.localPosition.y,
                        flashRoot.localPosition.z);
                }
                flashRoot.localScale = new Vector3(-flashRoot.localScale.x,
                    flashRoot.localScale.y,
                    flashRoot.localScale.z);
            }
            else
            {
                door.GetComponent<DoorKnockDoor>().MissKnock(transform.position.x);
            }
        }
        
        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f)
            {
                flashTimer = 0f;
                flash.SetActive(false);
            }
        }
    } 

    //OnTriggerStay2D doesn't work as well
    void OnTriggerEnter2D(Collider2D other){
        intersecting = true;
    }
    void OnTriggerExit2D(Collider2D other){
        intersecting = false;
    }
}
