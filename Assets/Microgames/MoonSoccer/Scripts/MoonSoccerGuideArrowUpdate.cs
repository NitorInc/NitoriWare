using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonSoccerGuideArrowUpdate : MonoBehaviour {

    [Header("Blinking Speed")]
    [SerializeField]
    private float blinkSpeed = 0.2f;
    [Header("Blinking Duration")]
    [SerializeField]
    private float blinkDuration = 1f;

    private float blinkTimer = 0f;

	// Use this for initialization
	void Start () {
		Invoke("DestroySelf", blinkDuration);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Space))
            Destroy(gameObject);
        
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
