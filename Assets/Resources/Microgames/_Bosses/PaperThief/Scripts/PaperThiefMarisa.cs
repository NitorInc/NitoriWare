using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefMarisa : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private State state;
    [SerializeField]
    private float starFireCooldown, hitFlashSpeed, hitFlashColorDrop;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private GameObject starPrefab;
    [SerializeField]
    private Transform starCreationPoint;
    [SerializeField]
    private ParticleSystem broomParticles;
#pragma warning restore 0649

    private SpriteRenderer[] _spriteRenderers;
    private SineWave _sineWave;
    private bool flashingRed;
    private float starFireTimer;
    
    public enum State
    {
        Cutscene,
        Fight,
        Defeat
    }

    public enum QueueAnimation
    {
        Idle,       //0
        Snap,       //1
        Hurt        //2
    }

	void Awake()
	{
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _sineWave = GetComponent<SineWave>();
	}

    void Start()
    {
        var broomModule = broomParticles.main;
        broomModule.simulationSpace = ParticleSystemSimulationSpace.Custom;
        broomModule.customSimulationSpace = PaperThiefCamera.instance.transform;

        ChangeState(state);
    }

    void ChangeState(State state)
    {
        switch(state)
        {
            case (State.Fight):
                starFireTimer = starFireCooldown;
                break;
            default:
                break;
        }
        this.state = state;
    }
	
	void Update()
	{
        switch(state)
        {
            case (State.Fight):
                updateFight();
                break;
            default:
                break;
        }
	}

    void updateFight()
    {
        starFireTimer -= Time.deltaTime;
        if (starFireTimer <= 0f)
        {
            queueAnimation(QueueAnimation.Snap);
            starFireTimer = starFireCooldown;
        }
        if (flashingRed || _spriteRenderers[0].color.b < 1f)
            updateHitFlash();
    }

    void updateHitFlash()
    {
        Color color = _spriteRenderers[0].color;
        float currentB = color.b, diff = hitFlashSpeed * Time.deltaTime;
        if (flashingRed)
        {
            if (currentB - diff <= hitFlashColorDrop)
            {
                color.g = color.b = hitFlashColorDrop;
                flashingRed = false;
            }
            else
            {
                color.g = color.b = currentB - diff;
            }

        }
        else
        {
            if (currentB + diff >= 1f)
            {
                color.g = color.b = 1f;
            }
            else
            {
                color.g = color.b = currentB + diff;
            }
        }
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].color = color;
        }
    }

    public void createStar()
    {
        GameObject newStar = GameObject.Instantiate(starPrefab, starCreationPoint.position, Quaternion.identity);
        newStar.transform.parent = transform.parent;
        PaperThiefStar newStarComponent = newStar.GetComponent<PaperThiefStar>();
        if (_sineWave.enabled)
        {
            if (transform.localPosition.y < 0f)
                newStarComponent.forceAngleDirection = -1f;
            else if (transform.localPosition.x < 0f)
                newStarComponent.forceAngleDirection = 1f;
        }
        newStarComponent.forceAngleDirection = _sineWave.enabled ? (transform.position.y > 0f ? -1f : 1f) : 0f;
    }
    void queueAnimation(QueueAnimation animation)
	{
		rigAnimator.SetInteger("QueuedAnimation", (int)animation);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!PaperThiefNitori.dead && other.name.Contains("Shot"))
        {
            queueAnimation(QueueAnimation.Hurt);
            other.GetComponent<PaperThiefShot>().kill();
            flashingRed = true;
        }
    }
}
