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
            return this.pulse;
        }
        set
        {
            this.pulse = value;
            this.Reset();
        }
    }

    void Start()
    {
        this.Reset();
    }

	void Update()
    {
		if (this.pulse)
        {
            this.DoPulse();
        }
	}

    void Reset()
    {
        this.startTime = Time.time;
        this.DoPulse();
    }

    void DoPulse()
    {
        float pulseAmount = Mathf.Sin(((Time.time - this.startTime) * this.pulseSpeed) + Mathf.PI);
        pulseAmount = (pulseAmount + 1) / 2;

        float newScaleWidth = 1 + (pulseAmount * (this.maxWidthScale - 1));
        float newScaleHeight = 1 + (pulseAmount * (this.maxHeightScale - 1));

        this.transform.localScale = new Vector3(newScaleWidth, newScaleHeight);
    }

}
