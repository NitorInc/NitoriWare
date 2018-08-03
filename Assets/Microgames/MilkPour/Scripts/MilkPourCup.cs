using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPourCup : MonoBehaviour
{
    public float MaximumFill = 100;
    public float RequiredFill = 70;
    public float Fill;

    [SerializeField]
    private Transform _maskTransform;

    [SerializeField]
    private float _maskMinY;

    [SerializeField]
    private float _maskMaxY;

    [SerializeField]
    private Transform _fillLineTransform;

    void Start ()
    {
        Fill = 0;
        _fillLineTransform.localPosition = new Vector2 (_fillLineTransform.localPosition.x, RequiredFill / MaximumFill);
    }

    void Update ()
    {
        _maskTransform.localPosition = new Vector2 (_maskTransform.localPosition.x,
            Mathf.Lerp (_maskMinY, _maskMaxY, Fill / MaximumFill));
    }
}
