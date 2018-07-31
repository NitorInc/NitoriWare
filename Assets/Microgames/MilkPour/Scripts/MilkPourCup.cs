using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPourCup : MonoBehaviour {

	public const float MAX_FILL = 100;

	public float Fill;
	public float RequiredFill;

	[SerializeField]
	Transform _maskTransform;

	[SerializeField]
	float _maskMinY;

	[SerializeField]
	float _maskMaxY;

	[SerializeField]
	Transform _fillLineTransform;

	void Start () {
		Fill = 0;
		_fillLineTransform.localPosition = new Vector2 (_fillLineTransform.localPosition.x, RequiredFill / MAX_FILL);
	}

	void Update () {
		_maskTransform.localPosition = new Vector2 (_maskTransform.localPosition.x,
			Mathf.Lerp (_maskMinY, _maskMaxY, Fill / MAX_FILL));
	}
}
