using UnityEngine;

public class MilkPourGrowingCup : MilkPourCup
{
    [SerializeField]
    private float _initialMaxFill = 100;

    [SerializeField]
    private float _terminalMaxFill = 175;

    [SerializeField]
    private float _fillBoundsSize = 25;

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
    private Transform _fillLineTransform;

    private float _fill;
    private float _maxFill;
    private bool _stopped;
    private float _growthTimer;

    void Start ()
    {
        _fill = 0;
        _maxFill = _initialMaxFill;
        _stopped = false;
        _growthTimer = 0;
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

        _growthTimer += Time.deltaTime;
        _maxFill = Mathf.Lerp (_initialMaxFill, _terminalMaxFill, Mathf.Min(_growthTimer / _growthTime, 1f));

        _cupMaskTransform.localPosition = new Vector2 (_cupMaskTransform.localPosition.x,
            Mathf.Lerp (_cupMaskMinY, _cupMaskMaxY, _growthTimer / _growthTime));

        _fillMaskTransform.localPosition = new Vector2 (_fillMaskTransform.localPosition.x,
            Mathf.Lerp (_fillMaskMinY, _fillMaskMaxY, _fill / _terminalMaxFill));
    }

    public override void AddFill (float amount)
    {
        _fill += amount;
    }

    public override bool IsOverfilled ()
    {
        return _fill >= _maxFill;
    }

    public override bool IsFillReqMet ()
    {
        return _fill >= _maxFill - _fillBoundsSize;
    }

    public override void Stop ()
    {
        _stopped = true;
    }
}
