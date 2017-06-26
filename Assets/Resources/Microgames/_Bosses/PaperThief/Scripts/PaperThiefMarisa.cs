using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefMarisa : MonoBehaviour
{

    public static bool defeated;

#pragma warning disable 0649
    [SerializeField]
    private State state;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private float starFireCooldown, hitFlashSpeed, hitFlashColorDrop, defeatSpinFrequency, defeatMoveCenterTime;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private PaperThiefSpin spin;
    [SerializeField]
    private GameObject starPrefab;
    [SerializeField]
    private Transform starCreationPoint;
    [SerializeField]
    private ParticleSystem broomParticles, defeatedParticles;
#pragma warning restore 0649

    private SpriteRenderer[] _spriteRenderers;
    private SineWave _sineWave;
    private bool flashingRed;
    private float starFireTimer, defeatSpinTimer, moveCenterSpeed;
    private int health;
    
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
        defeated = false;
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
                health = maxHealth;
                MicrogameController.instance.displayCommand("Defeat her!");
                break;
            case (State.Defeat):
                PaperThiefNitori.instance.hasControl = false;
                _sineWave.enabled = false;
                defeatSpinTimer = 0f;
                moveCenterSpeed = ((Vector2)transform.localPosition).magnitude / defeatMoveCenterTime;
                defeatedParticles.Play();

                defeated = true;
                break;
            default:
                break;
        }
        rigAnimator.SetInteger("State", (int)state);
        this.state = state;
    }
	
	void Update()
	{
        switch(state)
        {
            case (State.Fight):
                updateFight();
                break;
            case (State.Defeat):
                updateDefeat();
                break;
            default:
                break;
        }

        if (flashingRed || _spriteRenderers[0].color.b < 1f)
            updateHitFlash();
    }

    void LateUpdate()
    {
        if (PaperThiefNitori.dead)
            stop();
    }

    void updateFight()
    {
        starFireTimer -= Time.deltaTime;
        if (starFireTimer <= 0f)
        {
            queueAnimation(QueueAnimation.Snap);
            starFireTimer = starFireCooldown;
        }
    }

    void updateDefeat()
    {
        defeatSpinTimer -= Time.deltaTime;
        if (defeatSpinTimer < 0f)
        {
            spin.facingRight = !spin.facingRight;
            defeatSpinTimer = defeatSpinFrequency;
        }

        if (transform.moveTowardsLocal(Vector2.zero, moveCenterSpeed))
        {
            spin.facingRight = false;
            defeatedParticles.Stop();
            enabled = false;
        }
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

    void stop()
    {
        rigAnimator.enabled = false;
        var broomModule = broomParticles.main;
        broomModule.simulationSpeed = 0f;
        _sineWave.enabled = false;
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!defeated && !PaperThiefNitori.dead && other.name.Contains("Shot"))
        {
            other.GetComponent<PaperThiefShot>().kill();

            queueAnimation(QueueAnimation.Hurt);
            flashingRed = true;
            health--;
            if (health <= 0)
                ChangeState(State.Defeat);
        }
    }
}
