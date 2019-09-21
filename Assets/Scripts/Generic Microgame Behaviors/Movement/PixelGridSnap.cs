using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelGridSnap : MonoBehaviour
{

    [SerializeField]
    private float pixelsPerUnit = 16f;
    
	
	void LateUpdate ()
    {
        var unitsPerPixel = 1f / pixelsPerUnit;
        var parentPos = transform.parent.position;

        parentPos.x = roundNearest(parentPos.x, unitsPerPixel);
        parentPos.y = roundNearest(parentPos.y, unitsPerPixel);

        transform.position = parentPos;
    }

    float roundNearest(float x, float m)
    {
        var mod = MathHelper.trueMod(x, m);
        var roundedDown = x - mod;
        if (mod >= m / 2f)
            return roundedDown + m;
        else
            return roundedDown;
    }
}
