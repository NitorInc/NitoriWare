using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerClipDrag : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer SR;

    [SerializeField]
    private GameObject Shadow;

    [SerializeField]
    private float snapBackSpeed = 20f;

    private Vector3 initialPosition;
    private bool dragging = false;
    bool hasBeenDragged = false;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void OnMouseDown()
    {
        SR.maskInteraction = 0;
        Shadow.SetActive(true);
        dragging = true;
    }

    private void OnMouseUp()
    {
        Shadow.SetActive(false);
        dragging = false;
    }

    private void Update()
    {
        if (dragging)
            hasBeenDragged = true;
        if (!hasBeenDragged)
        {
            initialPosition = transform.localPosition;
            return;
        }

        if (!dragging && tag == "MicrogameTag1")
            transform.moveTowardsLocal2D(initialPosition, snapBackSpeed);
    }

    void OnMouseDrag()
    {
        Vector3 NewPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = Camera.main.ScreenToWorldPoint(NewPos) + new Vector3 (0, 0, 10);
    }

}
