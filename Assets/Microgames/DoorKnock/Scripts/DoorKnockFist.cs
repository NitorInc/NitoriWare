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

    private float timer = 0;
    private bool intersecting = false;
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
            transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f)*Time.timeScale;
            if (transform.localScale.x <= 0){
                gameObject.SetActive(false);
            }
        } else if (Input.GetMouseButtonDown(0) && timer <= 0) {
            anim.SetTrigger("Knock");
            timer = coolDown;
            if (intersecting) {
                door.SendMessage("OnClick");
                particleSystem.Play();
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
