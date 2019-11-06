using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerClipDrag : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer SR;

    [SerializeField]
    private GameObject Shadow;

    private void OnMouseDown()
    {
        SR.maskInteraction = 0;
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
