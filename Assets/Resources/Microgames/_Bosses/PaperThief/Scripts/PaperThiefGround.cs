using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PaperThiefGround : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private float leftHeight, rightHeightOffset;
    [SerializeField]
    private BoxCollider2D _collider;
    [SerializeField]
    private RectTransform tile, top, topLeft, left, bottomLeft, topRight, right, bottomRight;
#pragma warning restore 0649

    private RectTransform _rectTransform;

	void Start()
	{
        _rectTransform = GetComponent<RectTransform>();
        _collider = GetComponent<BoxCollider2D>();
	}
	
	void Update()
	{
        if (Application.isPlaying)
            return;

        Vector2 size = _rectTransform.rect.size, half = size / 2f;

        _collider.size = size;
        _collider.offset = new Vector2(Mathf.Lerp(half.x, -half.x, _rectTransform.pivot.x),
            Mathf.Lerp(half.y, -half.y, _rectTransform.pivot.y));

        if (tile != null)
        {
            tile.sizeDelta = new Vector2(size.x, size.y - 1f);
            tile.localPosition = new Vector3(0f, -1f, 0f);
        }

        if (top != null)
        {
            top.sizeDelta = new Vector2(_rectTransform.sizeDelta.x - 2f, 1f);
            top.localPosition = Vector3.zero;
        }
        
        if (topLeft != null)
            topLeft.localPosition = new Vector3(-half.x, 0f, 0f);
        if (topRight != null)
            topRight.localPosition = new Vector3(half.x, 0f, 0f);

        if (left != null)
        {
            left.localPosition = new Vector3(-half.x, -half.y - .5f, 0f);
            left.sizeDelta = new Vector2(1f, _rectTransform.sizeDelta.y - 1f);
        }
        if (right != null)
        {
            right.localPosition = new Vector3(half.x, -half.y - .5f, 0f);
            right.sizeDelta = new Vector2(1f, _rectTransform.sizeDelta.y - 1f);
        }
        
        if (bottomLeft != null)
            bottomLeft.transform.localPosition = new Vector3(-half.x, -size.y);
        if (bottomRight != null)
            bottomRight.transform.localPosition = new Vector3(half.x, -size.y);
    }
}
