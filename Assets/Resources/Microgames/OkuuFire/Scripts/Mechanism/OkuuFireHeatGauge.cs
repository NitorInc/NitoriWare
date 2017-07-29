using UnityEngine;

public class OkuuFireHeatGauge : MonoBehaviour, IOkuuFireMechanism
{
    [Header("Method of victory")]
    public bool heatControl = false;
    public float maxHeatSpeed = 0.5F;

    [Header("How quickly you win once in the safe zone")]
    public float stabilizeSpeed = 1;
    [HideInInspector]
    public float destabilizeSpeed = 1;

    [Header("How much the fire can rise")]
    public float fireGrowRate = 1;

    [Header("Stage settings")]
    public Transform start;
    public float height;

    public Transform indicator;
    public SpriteRenderer targetZone;
    public ParticleSystem fire;

    public GameObject[] victoryObjects;

    private float heatSpeed;
    private float heatLevel;
    private float targetStartLevel;
    private float targetLevel;
    private float stability;

    // Cosmetic
    private float minFireHeight;

    private bool victory;

	void Awake()
    {
        // Set start temperature
        if (heatControl)
        {
            float startLevel = 0.1F + UnityEngine.Random.Range(0F, 0.4F);
            this.SetLevel(startLevel);
            this.heatSpeed = this.maxHeatSpeed;
        }

        // Randomly determine a target temperature
        float targetLevel = 0.5F - UnityEngine.Random.Range(0F, 0.38F);
        this.SetTarget(targetLevel);
        this.targetStartLevel = targetLevel;

        this.minFireHeight = fire.main.gravityModifierMultiplier;
    }
    
    void Update()
    {
        if (!this.victory)
        {
            if (stability >= 1)
                this.DoVictory();
            else
            {
                if (this.heatControl)
                {
                    // Move the heat indicator based on time and heat speed
                    float newHeat = this.heatLevel + (Time.deltaTime * this.heatSpeed);
                    this.SetLevel(newHeat);
                }
                
                // Stabilize when in target zone
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
        if (this.heatControl)
        {
            float heatRange = this.maxHeatSpeed * 1.75F;

            // Adjust heating speed based on completion amount
            this.heatSpeed = this.maxHeatSpeed - (heatRange * completion);
        }
        else
        {
            this.SetLevel(completion);
        }

        // Set flame height
        ParticleSystem.MainModule fireSettings = fire.main;
        fireSettings.gravityModifierMultiplier = this.minFireHeight - ((1 - completion) * this.fireGrowRate);
    }

    public float GetTargetOffset()
    {
        float currentPosition = this.indicator.localPosition.y;
        float targetPosition = this.targetZone.transform.localPosition.y;
        
        return targetPosition - currentPosition;
    }

    public bool InTargetZone()
    {
        Vector3 currentPosition = this.targetZone.transform.position;
        currentPosition.y = this.indicator.position.y;
        bool inZone = targetZone.GetComponent<Collider2D>().bounds.Contains(currentPosition);

        return inZone;
    }

    float GetGaugePositionY(float level)
    {
        // Invert
        level = 1 - level;

        float gaugeY = this.height * level;
        Vector3 gaugeStart = start.localPosition;
        gaugeY = gaugeStart.y + gaugeY;
        
        return gaugeY;
    }

    void SetLevel(float level)
    {
        if (level > 1)
            level = 1;

        Vector3 newGuagePosition = this.indicator.localPosition;
        newGuagePosition.y = GetGaugePositionY(level);

        this.indicator.localPosition = newGuagePosition;
        this.heatLevel = level;
    }

    void SetTarget(float level)
    {
        Vector3 newGuagePosition = this.targetZone.transform.localPosition;
        newGuagePosition.y = GetGaugePositionY(level);

        this.targetZone.transform.localPosition = newGuagePosition;
        SineWave wave = targetZone.GetComponent<SineWave>();
        if (wave != null)
        {
            wave.resetStartPosition();
            wave.yOffset = Random.Range(0f, 1f);
        }
        this.targetLevel = level;
    }

    void DoVictory()
    {
        MicrogameController.instance.setVictory(true, true);
        this.victory = true;

        foreach (GameObject victoryObject in this.victoryObjects)
        {
            victoryObject.SendMessage("Victory");
        }
        SineWave wave = targetZone.GetComponent<SineWave>();
        if (wave != null)
            wave.enabled = false;
    }
}
