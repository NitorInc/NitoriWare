using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerGuideArrowUpdate : MonoBehaviour {

    [Header("Blinking Speed")]
    [SerializeField]
    private float blinkSpeed = 0.2f;

    private float blinkTimer = 0f;

	// Use this for initialization
	void Start () {
		Invoke("DestroySelf", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkSpeed)
        {
            blinkTimer = 0f;
            this.gameObject.GetComponent<SpriteRenderer> ().enabled = ! this.gameObject.GetComponent<SpriteRenderer> ().enabled;
        }
	}
    
    void DestroySelf () {
        Destroy(gameObject);
    }
    
}
