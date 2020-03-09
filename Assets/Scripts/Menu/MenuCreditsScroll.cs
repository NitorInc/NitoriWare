using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCreditsScroll : MonoBehaviour
{
    [SerializeField]
    private Transform bottomAnchor;
    [SerializeField]
    private float scrollSpeed = 5f;
    [SerializeField]
    private float bottomAnchorLoopPosition = 5f;

    private float initialY;

    void Start ()
    {
        initialY = transform.position.y;
	}
	
	void Update ()
    {
        transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        if (bottomAnchor.transform.position.y > bottomAnchorLoopPosition)
            transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
	}
}
