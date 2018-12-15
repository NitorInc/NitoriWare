using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurBallController : MonoBehaviour {
    [SerializeField]
    GameObject sprite;
    public float speed = 0.3f;
    
    private bool shouldShrink = false;
    private bool hasMoved = false;
    private bool finished = false;
    private Transform t;
    void Start(){
        t = sprite.transform;
    }
    void Update(){
        if (!hasMoved && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))){
            hasMoved = true;
        }
        if (finished && t.localScale.x > 0) {
            float s = 3*Time.deltaTime*speed;
            t.localScale -= new Vector3(s, s, s);
            if (t.localScale.x <= 0){
                GetComponent<ParticleSystem>().Play();
                gameObject.tag = "Finish";
                sprite.GetComponent<Renderer>().enabled = false;
                t.localScale = new Vector3(0f, 0f, 0f);
            }
        } else if (gameObject.tag != "Finish" && shouldShrink && hasMoved){
            float s = Time.deltaTime*speed;
            t.localScale -= new Vector3(s, s, s);
            if (t.localScale.x <= 0.06){
                finished = true;
            }
        }
    }

    void OnTriggerEnter2D (Collider2D collider) { 
        if (collider.gameObject.tag == "Player") shouldShrink = true; 
    }
    void OnTriggerExit2D (Collider2D collider) { 
        if (collider.gameObject.tag == "Player") shouldShrink = false; 
    }
}
