using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PaperThiefGround : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private BoxCollider2D _collider;
    [SerializeField]
    private RectTransform top, topLeft, topRight, left, right, bottomLeft, bottomRight;
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

        top.sizeDelta = new Vector2(_rectTransform.sizeDelta.x - 2f, 1f);
        top.localPosition = Vector3.zero;
        
        topLeft.localPosition = new Vector3(-half.x, 0f, 0f);
        topRight.localPosition = topLeft.localPosition * -1f;

        left.localPosition = new Vector3(-half.x, (-half.y) - .5f, 0f);
        right.localPosition = new Vector3(-left.localPosition.x, left.localPosition.y, 0f);
        left.sizeDelta = right.sizeDelta = new Vector2(1f, _rectTransform.sizeDelta.y - 1f);

        bottomLeft.transform.localPosition = new Vector3(-half.x, -size.y);
        bottomRight.transform.localPosition = new Vector3(-bottomLeft.localPosition.x, bottomLeft.localPosition.y, 0f);
    }
}
