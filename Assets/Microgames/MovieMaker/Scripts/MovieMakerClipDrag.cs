using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieMakerClipDrag : MonoBehaviour
{

    void OnMouseDrag()
    {
        Vector3 NewPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        transform.position = Camera.main.ScreenToWorldPoint(NewPos) + new Vector3 (0, 0, 10);
    }

}
