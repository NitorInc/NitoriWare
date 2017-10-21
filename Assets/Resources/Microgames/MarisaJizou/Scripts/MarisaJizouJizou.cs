using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaJizouJizou : MonoBehaviour {

    MarisaJizouController controller;
    bool hasTouched = false;

    
    public GameObject happyExp;
    public GameObject sadExp;
    public GameObject hat;
    Collider2D col;

    public void Register(MarisaJizouController controller) {
        this.controller = controller;
        col = GetComponentInChildren<Collider2D>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!hasTouched) {
            controller.Notify(true);
            hasTouched = true;
            happyExp.SetActive(true);
            sadExp.SetActive(false);
            hat.SetActive(true);
            col.enabled = false;
            Destroy(collision.gameObject);
        }
    }
}
