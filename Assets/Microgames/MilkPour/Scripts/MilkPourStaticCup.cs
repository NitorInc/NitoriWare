using UnityEngine;

public class MilkPourStaticCup : MilkPourCup
{
    [SerializeField]
    private float _maxCupFill = 100;

    [SerializeField]
    private float _minFillLevel = 25f;

    [SerializeField]
    private float _maxFillLevel = 5;

    [SerializeField]
    private Transform _fillMaskTransform;

    [SerializeField]
    private float _fillMaskMinY;

    [SerializeField]
    private float _fillMaskMaxY;

    [SerializeField]
    private Transform _minFillLineTransform;

    [SerializeField]
    private Transform _maxFillLineTransform;

    private float _fill;

    private bool _stopped;

    void Start ()
    {
        _fill = 0;
        _stopped = false;

        float _fillOffset = _minFillLineTransform.localPosition.y;
        _minFillLineTransform.localPosition = new Vector2 (_minFillLineTransform.localPosition.x, _fillOffset - (_minFillLevel / _maxCupFill));
        _maxFillLineTransform.localPosition = new Vector2 (_maxFillLineTransform.localPosition.x, _fillOffset - (_maxFillLevel / _maxCupFill));
    }

    void Update ()
    {
        if (_stopped)
        {
            if (_fill > _maxCupFill)
                _fill = _maxCupFill;
            return;
        }

        _fillMaskTransform.localPosition = new Vector2 (_fillMaskTransform.localPosition.x,
            Mathf.Lerp (_fillMaskMinY, _fillMaskMaxY, _fill / _maxCupFill));
    }

    public override void AddFill(float amount)
    {
        _fill += amount;
    }

    public override bool IsFillMaxed()
    {
        return _fill >= _maxCupFill;
    }

    public override bool IsOverfilled()
    {
        return _fill > _maxCupFill - _maxFillLevel;
    }

    public override bool IsFillReqMet()
    {
        return _fill >= _maxCupFill - _minFillLevel && _fill <= _maxCupFill - _maxFillLevel;
    }

    public override void Stop()
    {
        _stopped = true;
    }
}
