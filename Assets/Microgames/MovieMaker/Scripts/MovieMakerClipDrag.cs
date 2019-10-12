using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerClipDrag : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer SR;

    [SerializeField]
    private Sprite Default;

    [SerializeField]
    private Sprite Over;

    [SerializeField]
    private GameObject Shadow;

    private void OnMouseEnter()
    {
        SR.sprite = Over;
    }

    private void OnMouseExit()
    {
        SR.sprite = Default;
    }

    private void OnMouseDown()
    {
        Shadow.SetActive(true);
    }

    private void OnMouseUp()
    {
        Shadow.SetActive(false);
    }


    void OnMouseDrag()
    {
        Vector3 NewPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = Camera.main.ScreenToWorldPoint(NewPos) + new Vector3 (0, 0, 10);
    }

}
