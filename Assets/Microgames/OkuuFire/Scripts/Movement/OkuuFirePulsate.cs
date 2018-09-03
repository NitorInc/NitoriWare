using UnityEngine;

public class OkuuFirePulsate : MonoBehaviour
{

    public float pulseSpeed;
    
    public float maxWidthScale;
    public float maxHeightScale;
    
    private float startTime;

    [SerializeField]
    private bool pulse;

    public bool Pulse
    {
        get
        {
            return pulse;
        }
        set
        {
            pulse = value;
            Reset();
        }
    }

    void Start()
    {
        Reset();
    }

	void Update()
    {
		if (pulse)
        {
            DoPulse();
        }
	}

    void Reset()
    {
        startTime = Time.time;
        DoPulse();
    }

    void DoPulse()
    {
        float pulseAmount = Mathf.Sin(((Time.time - startTime) * pulseSpeed) + Mathf.PI);
        pulseAmount = (pulseAmount + 1) / 2;

        float newScaleWidth = 1 + (pulseAmount * (maxWidthScale - 1));
        float newScaleHeight = 1 + (pulseAmount * (maxHeightScale - 1));

        transform.localScale = new Vector3(newScaleWidth, newScaleHeight);
    }

}
