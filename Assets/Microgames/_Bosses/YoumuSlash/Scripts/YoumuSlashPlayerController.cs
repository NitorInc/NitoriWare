using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashPlayerController : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private Transform facingTransform;
    [SerializeField]
    private Transform facingSpriteTransform;
    [SerializeField]
    private YoumuSlashSpriteTrail spriteTrail;
    [SerializeField]
    private float spriteTrailStartOffset;
    [SerializeField]
    private bool firstTargetStareMode;
    [SerializeField]
    private bool allowInput;
    public bool AllowInput
    {
        get { return allowInput; }
        set { allowInput = value; }
    }
    [SerializeField]
    private bool autoSlash;
    public bool AutoSlash
    {
        get { return autoSlash; }
        set { autoSlash = value; }
    }

    [Header("Timing window in seconds for hitting an object")]
    [SerializeField]
    private Vector2 hitTimeFudge;
    [Header("Minimum time to miss a slash after a previous attempt")]
    [SerializeField]
    private float slashCooldown = .5f;
    [SerializeField]
    private float missReactionAnimationTime = .5f;
    [SerializeField]
    private int upsetResetHitCount = 6;
    [SerializeField]
    private float slashAnimationEffectTime;
    [SerializeField]
    private float idleReturnAnimationDelay;

    [SerializeField]
    private Vector2 sliceAngleRange;
    [SerializeField]
    private AudioClip debugSound;
    [SerializeField]
    private AudioClip hitVoiceClip;
    [SerializeField]
    private AudioClip screamClip;
    
    int nextIdleBeat = -100;
    int untenseBeat = -1;
    bool attacking;
    int beatTriggerResetTimer;
    bool attackedUp;
    bool attackWasSuccess;
    YoumuSlashBeatMap.TargetBeat.Direction lastSliceDirection;
    float slashCooldownTimer;
    bool holdAttack;
    bool noteMissReactionQueued;
    YoumuSlashBeatMap.TargetBeat nextTarget;
    int upsetResetHits;
    float lastIdleTime;
    float lastAttackTime;

    private void Start()
    {
        YoumuSlashTimingController.onBeat += onBeat;
        YoumuSlashTargetSpawner.OnTargetLaunch += onTargetLaunched;
        nextTarget = getFirstActiveTarget();
    }

    void onTargetLaunched(YoumuSlashBeatMap.TargetBeat target)
    {
        if ((!firstTargetStareMode || timingData.BeatMap.getFirstActiveTarget(timingData.CurrentBeat, hitTimeFudge.y) == target)
            && !attacking
            && getFirstHittableTarget(YoumuSlashBeatMap.TargetBeat.Direction.Any) == null)
            rigAnimator.SetBool("LookBack", isFacingRight() != (target.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right));
        if (autoSlash)
            Invoke("performAutoSlash", (target.HitBeat - timingData.CurrentBeat) * timingData.BeatDuration);
    }

    void performAutoSlash()
    {
        attemptSlash(YoumuSlashBeatMap.TargetBeat.Direction.Any);
    }

    void onBeat(int beat)
    {
        if (beat >= nextIdleBeat &&
            (attackWasSuccess || slashCooldownTimer <= 0f))
        {
            handleIdleAnimation(beat);
        }
        rigAnimator.SetBool("IsAttacking", attacking);
        
        rigAnimator.SetTrigger("Beat");
        beatTriggerResetTimer = 2;
    }

    void handleIdleAnimation(int beat)
    {
        var nextTarget = timingData.BeatMap.getFirstActiveTarget((float)beat, hitTimeFudge.y);
        //var lastTarget = timingData.BeatMap.getLastActiveTarget((float)beat, hitTimeFudge.y);

        if (beat >= untenseBeat)
            rigAnimator.SetBool("Tense", false);

        if (beat == nextIdleBeat)
        {
            if (nextTarget != null && beat >= (int)nextTarget.HitBeat)
            {
                //Delay idle if new target in 1 beat
                nextIdleBeat++;
                return;
            }
            else
            {
                MicrogameController.instance.playSFX(debugSound);
                returnToIdle();
            }
        }
        if (beat >= nextIdleBeat)
        {
            //Start bobbing on return
            rigAnimator.SetTrigger("Bob");
        }

        if (nextTarget != null)
        {
            if (beat + 1 >= (int)nextTarget.HitBeat)
            {
                //1 Beat prep
                bool initialFlip = isFacingRight();
                bool flipIdle = nextTarget.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right;
                if (isRigFacingRight())
                    flipIdle = !flipIdle;
                setIdleFlipped(flipIdle);

                if (isFacingRight() != initialFlip)
                {
                    rigAnimator.SetBool("LookBack", false);
                    rigAnimator.SetTrigger("ResetLook");
                }
                rigAnimator.SetBool("Tense", true);
                untenseBeat = beat + 2;

                nextIdleBeat = beat + 2;
            }
            else if (beat + 2 >= (int)nextTarget.HitBeat)
            {
                //2 Beat prep
            }
        }
        attacking = false;
        rigAnimator.SetBool("Prep", false);
    }

    void returnToIdle()
    {
        if (attacking)
        {
            rigAnimator.ResetTrigger("Attack");
            rigAnimator.SetTrigger("Idle");
        }
        attacking = false;
        lastIdleTime = Time.time;
        spriteTrail.EnableSpawn = false;
        rigAnimator.SetBool("AttackUp", false);
    }

    //For animation purposes
    public void freezeInput(bool facingRight)
    {
        returnToIdle();
        if (isFacingRight() != facingRight)
            setIdleFlipped(!isIdleFlipped());
        rigAnimator.SetBool("LookBack", false);
        rigAnimator.SetBool("Upset", false);
        disableNoteMissReaction();
        allowInput = false;
    }
    
    public void callGoCommand()
    {
        MicrogameController.instance.displayLocalizedCommand("commandb", "Go!");
    }

    void setRigFacingRight(bool facingRight)
    {
        facingTransform.localScale = new Vector3(Mathf.Abs(facingTransform.localScale.x) * (facingRight ? -1f : 1f),
            facingTransform.localScale.y, facingTransform.localScale.z);
        rigAnimator.SetBool("FacingRight", facingRight);
    }

    void setIdleFlipped(bool flip)
    {
        facingSpriteTransform.localScale = new Vector3(Mathf.Abs(facingSpriteTransform.localScale.x) * (flip ? -1f : 1f),
            facingSpriteTransform.localScale.y, facingSpriteTransform.localScale.z);
    }

    bool isIdleFlipped()
    {
        return facingSpriteTransform.localScale.x < 0f;
    }

    bool isRigFacingRight()
    {
        return facingTransform.localScale.x < 0f;
    }

    bool isFacingRight()
    {
        bool spriteFlipped = facingSpriteTransform.localScale.x < 0f;
        return isRigFacingRight() ? !spriteFlipped : spriteFlipped;
    }

    public void setBobEnabled(bool enable)
    {
        rigAnimator.SetBool("EnableBob", enable);
    }

    public void setTenseEnabled(bool enable)
    {
        rigAnimator.SetBool("EnableTense", enable);
    }

    public void setForceTense(bool forceTense)
    {
        rigAnimator.SetBool("ForceTense", forceTense);
    }

    public void setEyesClosed(bool closed)
    {
        rigAnimator.SetBool("EyesClosed", closed);
    }

    void Update ()
    {
        if (beatTriggerResetTimer > 0)
        {
            beatTriggerResetTimer--;
            if (beatTriggerResetTimer <= 0)
            {
                rigAnimator.ResetTrigger("Beat");
            }
        }

        if (slashCooldownTimer > 0f)
        {
            slashCooldownTimer = Mathf.MoveTowards(slashCooldownTimer, 0f, Time.deltaTime);
            if (!attackWasSuccess && slashCooldownTimer <= 0f)
                returnToIdle();
        }
        if (allowInput)
            handleInput();
        
        var currentNextTarget = getFirstActiveTarget();
        if (nextTarget != currentNextTarget)
        {
            if (nextTarget != null && !nextTarget.slashed)
            {
                if (canReactToMissedNote())
                    playNoteMissReaction();
                else
                    noteMissReactionQueued = true;
                triggerMiss();
            }
            nextTarget = currentNextTarget;
        }
        else if (noteMissReactionQueued && canReactToMissedNote())
        {
            playNoteMissReaction();
            noteMissReactionQueued = false;
        }

    }

    bool canReactToMissedNote()
    {
        return !attacking && Time.time >= lastIdleTime + idleReturnAnimationDelay;
    }

    void handleInput()
    {
        var directionPressed = YoumuSlashBeatMap.TargetBeat.Direction.Any;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            directionPressed = YoumuSlashBeatMap.TargetBeat.Direction.Left;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            directionPressed = YoumuSlashBeatMap.TargetBeat.Direction.Right;

        if (directionPressed != YoumuSlashBeatMap.TargetBeat.Direction.Any)
        {
            attemptSlash(directionPressed);
        }
    }

    YoumuSlashBeatMap.TargetBeat getFirstHittableTarget(YoumuSlashBeatMap.TargetBeat.Direction direction)
    {
        return timingData.BeatMap.getFirstHittableTarget(timingData.CurrentBeat,
            hitTimeFudge.x / timingData.BeatDuration, hitTimeFudge.y / timingData.BeatDuration, direction);
    }

    YoumuSlashBeatMap.TargetBeat getFirstActiveTarget()
    {
        return timingData.BeatMap.getFirstActiveTarget(timingData.CurrentBeat,
            hitTimeFudge.y / timingData.BeatDuration);
    }

    void attemptSlash(YoumuSlashBeatMap.TargetBeat.Direction direction)
    {
        var hitTarget = getFirstHittableTarget(direction);
        bool isHit = hitTarget != null;
        if (holdAttack && !isHit && attacking)    //No slash if holdAttack is true and this slash attack is a miss and we're still attacking
            return;
        if (slashCooldownTimer > 0f //No slash if cooldown timer isn't reached and attack is a miss
            &&  (!isHit || !attackWasSuccess))   //Or if last attack was a miss
            return;
        else if (isHit)   //For force direction (auto-slash)
            direction = hitTarget.HitDirection;
        attackWasSuccess = isHit;
        slashCooldownTimer = slashCooldown;
        noteMissReactionQueued = false;
        lastAttackTime = Time.time;

        //Do animation stuff
        rigAnimator.SetBool("IsAttacking", true);
        bool reAttacking = attacking == true && direction == lastSliceDirection;
        rigAnimator.SetBool("ReAttack", reAttacking);
        lastSliceDirection = direction;
        bool attackingUp = (!attackedUp && attacking);
        if (hitTarget!= null && hitTarget.ForceUp)
            attackingUp = true;
        rigAnimator.SetBool("AttackUp", attackingUp);
        attackedUp = attackingUp;
        rigAnimator.SetBool("AttackMissed", !isHit);
        disableNoteMissReaction();

        attacking = true;
        if (!isHit)
            triggerMiss();

        bool facingRight = direction == YoumuSlashBeatMap.TargetBeat.Direction.Right;
        setRigFacingRight(facingRight);
        setIdleFlipped(false);
        rigAnimator.SetTrigger("Attack");
        rigAnimator.ResetTrigger("Idle");
        rigAnimator.SetBool("Tense", false);
        untenseBeat = -1;
        rigAnimator.SetBool("LookBack", false);
        rigAnimator.SetTrigger("ResetLook");
        float facingDirection = (isRigFacingRight() ? -1f : 1f);
        spriteTrail.resetTrail(spriteTrailStartOffset * facingDirection);
        
        rigAnimator.SetBool("Scream", false);
        holdAttack = false;
        if (isHit)
        {
            //Hit successful
            hitTarget.launchInstance.slash(MathHelper.randomRangeFromVector(sliceAngleRange), slashAnimationEffectTime);
            nextIdleBeat = (int)hitTarget.HitBeat + 1;
            if (upsetResetHits > 0)
            {
                upsetResetHits--;
                if (upsetResetHits == 0)
                    rigAnimator.SetBool("Upset", false);
            }

            switch (hitTarget.HitEffect)
            {
                case (YoumuSlashBeatMap.TargetBeat.Effect.Scream):
                    rigAnimator.SetBool("Scream", true);
                    nextIdleBeat++;
                    MicrogameController.instance.playSFX(screamClip);
                    holdAttack = true;
                    break;
                default:
                    MicrogameController.instance.playSFX(hitVoiceClip, pitchMult: Random.Range(.95f, 1.05f));
                    break;
            }
        }
        else
        {
            //Missed
            MicrogameController.instance.playSFX(hitVoiceClip, pitchMult: Random.Range(.95f, 1.05f));
        }

        spriteTrail.EnableSpawn = isHit ? (!reAttacking) : false;
    }

    void triggerMiss()
    {
        upsetResetHits = upsetResetHitCount;
        rigAnimator.SetBool("Upset", true);
    }

    void playNoteMissReaction()
    {
        rigAnimator.SetBool("NoteMissed", true);
        CancelInvoke("disableNoteMissReaction");
        Invoke("disableNoteMissReaction", missReactionAnimationTime);
    }

    void disableNoteMissReaction()
    {
        rigAnimator.SetBool("NoteMissed", false);
        CancelInvoke("disableNoteMissReaction");
    }
}
