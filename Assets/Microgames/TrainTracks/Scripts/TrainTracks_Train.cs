using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_Train : MonoBehaviour {

    private float initialx;
    private float initialy;
    private float initialscale;

    private float xincrement;
    private float yincrement;
    private float scaleincrement;

    private int frame;

    [SerializeField]
    private float finalx;

    [SerializeField]
    private float finaly;

    [SerializeField]
    private float finalscale;

    [SerializeField]
    private int travelframes;

    // Use this for initialization
    void Start () {
        initialx = transform.position.x;
        initialy = transform.position.y;
        initialscale = transform.localScale.x;

        xincrement = (finalx - initialx) * (1f / travelframes);
        yincrement = (finaly - initialy) * (1f / travelframes);
        scaleincrement = (finalscale - initialscale) * (1f / travelframes);
    }
	
	// Update is called once per frame
	void Update () {
        xincrement *= 1 + (10f / travelframes);
        yincrement *= 1 + (10f / travelframes);
        scaleincrement *= 1 + (10f / travelframes);
        transform.position = new Vector3(transform.position.x + xincrement, transform.position.y + yincrement, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x + scaleincrement, transform.localScale.y + scaleincrement, transform.localScale.z);
	}
}
