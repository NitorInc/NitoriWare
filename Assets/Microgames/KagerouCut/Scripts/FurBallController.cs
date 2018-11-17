using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurBallController : MonoBehaviour {
    [SerializeField]
    GameObject sprite;
    public float speed = 0.01f;
    
    private bool shouldShrink = false;
    private bool hasMoved = false;
    private bool removed = false;
    private Transform t;
    void Start(){
        t = sprite.transform;
    }
    void Update(){
        if (!hasMoved && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))){
            hasMoved = true;
        }
        if (gameObject.tag != "Finish" && shouldShrink && hasMoved){
            if (t.localScale.x <= 0){
                GetComponent<ParticleSystem>().Play();
                gameObject.tag = "Finish";
                print("finish");
                sprite.GetComponent<Renderer>().enabled = false;
                t.localScale = new Vector3(0f, 0f, 0f);
            }
            t.localScale -= new Vector3(speed, speed, speed);
        }
    }

    void OnTriggerEnter2D (Collider2D collider) { 
        if (collider.gameObject.tag == "Player") shouldShrink = true; 
    }
    void OnTriggerExit2D (Collider2D collider) { 
        if (collider.gameObject.tag == "Player") shouldShrink = false; 
    }
}
