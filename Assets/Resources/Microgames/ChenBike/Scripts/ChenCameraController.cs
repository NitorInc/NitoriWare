using UnityEngine;
using System.Collections;

public class ChenCameraController : MonoBehaviour {
    public float speed = 1f;
    public float divider = 4f;
    private Vector3 newPosition;

	// Use this for initialization
	void Start () {
        newPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        newPosition += (Time.deltaTime * speed, -Time.deltaTime * speed/divider, 0f);
        transform.position = newPosition;
	}
}
