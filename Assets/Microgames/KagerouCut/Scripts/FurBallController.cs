using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurBallController : MonoBehaviour {
    [SerializeField]
    GameObject sprite;
    public float speed = 0.01f;
    
    private bool shouldShrink = false;
    private Transform t;
    private bool exists = true;
    void Start(){
        t = sprite.transform;
    }
    void Update(){
        if (shouldShrink){
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
