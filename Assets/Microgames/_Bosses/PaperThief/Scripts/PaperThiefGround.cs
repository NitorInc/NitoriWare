using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PaperThiefGround : MonoBehaviour
{

    private RectTransform rectTransform;
    private BoxCollider2D boxCollider;

	void Start()
	{
        if (Application.isPlaying)
        {
            enabled = false;
            return;
        }

        rectTransform = (RectTransform)transform;
        boxCollider = GetComponent<BoxCollider2D>();
	}

    void Update()
    {
        boxCollider.offset = new Vector2(0f, -rectTransform.sizeDelta.y / 2f);
        boxCollider.size = rectTransform.sizeDelta;
    }
}
