using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurBallController : MonoBehaviour {
    [SerializeField]
    GameObject sprite;
    public float speed = 0.01f;
    
    private bool shouldShrink = false;
    private bool hasMoved = false;
    private Transform t;
    void Start(){
        t = sprite.transform;
    }
    void Update(){
        if (!hasMoved && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))){
            hasMoved = true;
        }
        if (shouldShrink && hasMoved){
            if (t.localScale.x <= 0){
                Destroy(this.gameObject); 
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
