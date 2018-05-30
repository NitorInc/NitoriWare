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

    public SpriteRenderer indicatorHighlight;

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
            SetLevel(startLevel);
            heatSpeed = maxHeatSpeed;
        }

        // Randomly determine a target temperature
        float targetLevel = 0.5F - UnityEngine.Random.Range(0F, 0.4F);
        SetTarget(targetLevel);
        targetStartLevel = targetLevel;

        minFireHeight = fire.main.gravityModifierMultiplier;
    }
    
    void Update()
    {
        if (!victory)
        {
            if (stability >= 1)
                DoVictory();
            else
            {
                if (heatControl)
                {
                    // Move the heat indicator based on time and heat speed
                    float newHeat = heatLevel + (Time.deltaTime * heatSpeed);
                    SetLevel(newHeat);
                }

                // Stabilize when in target zone
                if (InTargetZone())
                {
                    stability += Time.deltaTime * stabilizeSpeed;

                    indicatorHighlight.enabled = true;
                }
                else
                {
                    indicatorHighlight.enabled = false;

                    if (stability > 0)
                        stability -= Time.deltaTime * destabilizeSpeed;
                    else
                        stability = 0;
                }
            }
        }
    }

    public void Move(float completion)
    {
        if (heatControl)
        {
            float heatRange = maxHeatSpeed * 1.75F;

            // Adjust heating speed based on completion amount
            heatSpeed = maxHeatSpeed - (heatRange * completion);
        }
        else
        {
            SetLevel(completion);
        }

        // Set flame height
        ParticleSystem.MainModule fireSettings = fire.main;
        fireSettings.gravityModifierMultiplier = minFireHeight - ((1 - completion) * fireGrowRate);
    }

    public float GetTargetOffset()
    {
        float currentPosition = indicator.localPosition.y;
        float targetPosition = targetZone.transform.localPosition.y;
        
        return targetPosition - currentPosition;
    }

    public bool InTargetZone()
    {
        Vector3 currentPosition = targetZone.transform.position;
        currentPosition.y = indicator.position.y;
        bool inZone = targetZone.GetComponent<Collider2D>().bounds.Contains(currentPosition);

        return inZone;
    }

    float GetGaugePositionY(float level)
    {
        // Invert
        level = 1 - level;

        float gaugeY = height * level;
        Vector3 gaugeStart = start.localPosition;
        gaugeY = gaugeStart.y + gaugeY;
        
        return gaugeY;
    }

    void SetLevel(float level)
    {
        if (level > 1)
            level = 1;

        Vector3 newGuagePosition = indicator.localPosition;
        newGuagePosition.y = GetGaugePositionY(level);

        indicator.localPosition = newGuagePosition;
        heatLevel = level;
    }

    void SetTarget(float level)
    {
        Vector3 newGuagePosition = targetZone.transform.localPosition;
        newGuagePosition.y = GetGaugePositionY(level);

        targetZone.transform.localPosition = newGuagePosition;
        SineWave wave = targetZone.GetComponent<SineWave>();
        if (wave != null)
        {
            wave.resetStartPosition();
            wave.yOffset = Random.Range(0f, 1f);
        }
        targetLevel = level;
    }

    void DoVictory()
    {
        MicrogameController.instance.setVictory(true, true);
        victory = true;

        foreach (GameObject victoryObject in victoryObjects)
        {
            victoryObject.SendMessage("Victory");
        }
        SineWave wave = targetZone.GetComponent<SineWave>();
        if (wave != null)
            wave.enabled = false;
    }
}
