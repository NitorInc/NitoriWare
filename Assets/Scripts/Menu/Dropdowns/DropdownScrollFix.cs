using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownScrollFix : MonoBehaviour
{

    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private float initialZ;

	void Start()
	{
        rectTransform = (RectTransform)transform;
        initialPosition = rectTransform.anchoredPosition;
        initialZ = rectTransform.localPosition.z;
	}
	
	void LateUpdate()
	{
        if (float.IsNaN(rectTransform.anchoredPosition.x))
        {
            rectTransform.anchoredPosition = initialPosition;
            rectTransform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, initialZ);
        }
	}
}
