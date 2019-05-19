using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_Cursor : MonoBehaviour {

    [SerializeField]
    private Sprite closedsprite;

    [SerializeField]
    private Sprite opensprite;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            this.GetComponent<SpriteRenderer>().sprite = closedsprite;
        } else
        {
            this.GetComponent<SpriteRenderer>().sprite = opensprite;
        }

    }
}
