using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CommandDisplayLerpY : MonoBehaviour
{
    [SerializeField]
    private bool offsetParentY = true;
    [SerializeField]
    private float t;

    public float RestYPosition { get; set; }


    void LateUpdate()
    {
        var yAtOne = RestYPosition;
        if (offsetParentY)
        {
            yAtOne -= transform.parent.localPosition.y / transform.parent.localScale.y;
            yAtOne /= transform.parent.localScale.y;
        }
        var yPos = Mathf.Lerp(0f, yAtOne, t);
        transform.localPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.y);
    }
}
