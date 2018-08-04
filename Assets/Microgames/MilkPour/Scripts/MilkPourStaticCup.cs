using UnityEngine;

public class MilkPourStaticCup : MilkPourCup
{
    [SerializeField]
    private float _maxFill = 100;

    [SerializeField]
    private float _fillBoundsSize = 40;

    [SerializeField]
    private Transform _fillMaskTransform;

    [SerializeField]
    private float _fillMaskMinY;

    [SerializeField]
    private float _fillMaskMaxY;

    [SerializeField]
    private Transform _fillLineTransform;

    private float _fill;

    private bool _stopped;

    void Start ()
    {
        _fill = 0;
        _stopped = false;

        float _fillOffset = _fillLineTransform.localPosition.y;
        _fillLineTransform.localPosition = new Vector2 (_fillLineTransform.localPosition.x, _fillOffset - (_fillBoundsSize / _maxFill));
    }

    void Update ()
    {
        if (_stopped)
        {
            if (_fill > _maxFill)
                _fill = _maxFill;
            return;
        }

        _fillMaskTransform.localPosition = new Vector2 (_fillMaskTransform.localPosition.x,
            Mathf.Lerp (_fillMaskMinY, _fillMaskMaxY, _fill / _maxFill));
    }

    public override void AddFill(float amount)
    {
        _fill += amount;
    }

    public override bool IsOverfilled()
    {
        return _fill >= _maxFill;
    }

    public override bool IsFillReqMet()
    {
        return _fill >= _maxFill - _fillBoundsSize;
    }

    public override void Stop()
    {
        _stopped = true;
    }
}
