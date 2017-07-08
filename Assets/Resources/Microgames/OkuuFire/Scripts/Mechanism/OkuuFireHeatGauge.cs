using UnityEngine;

public class OkuuFireHeatGauge : MonoBehaviour, IOkuuFireMechanism
{
    public float stabilizeSpeed = 1;
    public float destabilizeSpeed = 1;

    public Transform start;
    public float height;

    public Transform indicator;
    public SpriteRenderer targetZone;

    public GameObject[] victoryObjects;
    
    private float stability;
    private bool victory;

	void Start ()
    {
        // Set start temperature
        this.SetLevel(0);

        // Randomly determine a target temperature
        float targetLevel = 0.2F + UnityEngine.Random.Range(0F, 0.8F);
        this.SetTarget(targetLevel);
	}
	
	void Update ()
    {
        if (!this.victory)
        {
            if (stability >= 1)
                this.DoVictory();
            else
            {
                if (this.InTargetZone())
                    stability += Time.deltaTime * this.stabilizeSpeed;
                else if (stability > 0)
                    stability -= Time.deltaTime * this.destabilizeSpeed;
                else
                    stability = 0;
            }
        }
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
    }

    bool InTargetZone()
    {
        Vector3 currentPosition = this.targetZone.transform.position;
        currentPosition.y = this.indicator.position.y;
        bool inZone = targetZone.GetComponent<Collider2D>().bounds.Contains(currentPosition);
        
        return inZone;
    }

    void DoVictory()
    {
        MicrogameController.instance.setVictory(true, true);
        this.victory = true;

        foreach (GameObject victoryObject in this.victoryObjects)
        {
            victoryObject.SendMessage("Victory");
        }
    }
}
