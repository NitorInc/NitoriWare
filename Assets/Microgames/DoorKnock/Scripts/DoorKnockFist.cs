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
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (timer > 0) timer -= Time.deltaTime;
        if (MicrogameController.instance.getVictory()){
            gameObject.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(0) && timer <= 0) {
            anim.SetTrigger("Knock");
            ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
            particleSystem.Play();
            timer = coolDown;
            if (intersecting) door.SendMessage("OnClick");
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
