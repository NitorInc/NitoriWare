using UnityEngine;

public class OkuuFireHeatGauge : MonoBehaviour, IOkuuFireMechanism
{
    public float stabilizeSpeed = 1;
    public float destabilizeSpeed = 1;

    public Transform start;
    public float height;

    public Transform indicator;
    public SpriteRenderer targetZone;

    public float targetLevel;
    
    public float stability;

	void Start ()
    {
        // Set start temperature
        this.SetLevel(0);

        // Randomly determine a target temperature
        this.SetTarget(UnityEngine.Random.Range(0F, 1F));
	}
	
	void Update ()
    {
        if (this.InTargetZone())
            stability += Time.deltaTime * this.stabilizeSpeed;
        else if (stability > 0)
            stability -= Time.deltaTime * this.destabilizeSpeed;
        else
            stability = 0;

        if (stability >= 1)
            MicrogameController.instance.setVictory(true, true);
    }

    public void Move(float completion)
    {
        this.SetLevel(completion);
    }

    float GetGaugePositionY(float level)
    {
        float gaugeY = this.height * level;
        Vector3 gaugeStart = start.localPosition;
        gaugeY = gaugeStart.y + gaugeY;
        
        return gaugeY;
    }

    void SetLevel(float level)
    {
        Vector3 newGuagePosition = this.indicator.localPosition;
        newGuagePosition.y = GetGaugePositionY(level);

        this.indicator.localPosition = newGuagePosition;
    }

    void SetTarget(float level)
    {
        Vector3 newGuagePosition = this.targetZone.transform.localPosition;
        newGuagePosition.y = GetGaugePositionY(level);

        this.targetZone.transform.localPosition = newGuagePosition;
        this.targetLevel = level;
    }

    bool InTargetZone()
    {
        Vector3 currentPosition = this.targetZone.transform.position;
        currentPosition.y = this.indicator.position.y;
        bool inZone = targetZone.GetComponent<Collider2D>().bounds.Contains(currentPosition);
        
        return inZone;
    }
}
