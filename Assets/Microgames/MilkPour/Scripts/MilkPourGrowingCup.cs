using UnityEngine;

public class MilkPourGrowingCup : MilkPourCup
{
    [SerializeField]
    private float _initialMaxCupFill = 100;

    [SerializeField]
    private float _terminalMaxCupFill = 175;

    [SerializeField]
    private float _minFillLevel = 22.5f;

    [SerializeField]
    private float _maxFillLevel = 7.5f;

    [SerializeField]
    private float _growthTime = 4;

    [SerializeField]
    private Transform _fillMaskTransform;

    [SerializeField]
    private float _fillMaskMinY;

    [SerializeField]
    private float _fillMaskMaxY;

    [SerializeField]
    private Transform _cupMaskTransform;

    [SerializeField]
    private float _cupMaskMinY;

    [SerializeField]
    private float _cupMaskMaxY;

    [SerializeField]
    private Transform _minFillLineTransform;

    [SerializeField]
    private Transform _maxFillLineTransform;

    private float _fill;
    private float _maxCupFill;
    private bool _stopped;
    private float _growthTimer;

    void Start ()
    {
        _fill = 0;
        _maxCupFill = _initialMaxCupFill;
        _stopped = false;
        _growthTimer = 0;
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

        _growthTimer += Time.deltaTime;
        _maxCupFill = Mathf.Lerp (_initialMaxCupFill, _terminalMaxCupFill, Mathf.Min(_growthTimer / _growthTime, 1f));

        _cupMaskTransform.localPosition = new Vector2 (_cupMaskTransform.localPosition.x,
            Mathf.Lerp (_cupMaskMinY, _cupMaskMaxY, _growthTimer / _growthTime));

        _fillMaskTransform.localPosition = new Vector2 (_fillMaskTransform.localPosition.x,
            Mathf.Lerp (_fillMaskMinY, _fillMaskMaxY, _fill / _terminalMaxCupFill));
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
