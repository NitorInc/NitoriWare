using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnockFist : MonoBehaviour {

	[SerializeField]
    private Sprite resting;

    [SerializeField]
    private Sprite knocking;
	
    private SpriteRenderer renderer;

    void Start() {
        renderer = transform.Find("Rig").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0)) {
            renderer.sprite = knocking;
        }
        else if(Input.GetMouseButtonUp(0)){
            renderer.sprite = resting;
        }
	}
}
