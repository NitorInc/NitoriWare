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
        newPosition.x += Time.deltaTime * speed;
        newPosition.y -= Time.deltaTime * speed / divider;
        transform.position = newPosition;
	}
}
