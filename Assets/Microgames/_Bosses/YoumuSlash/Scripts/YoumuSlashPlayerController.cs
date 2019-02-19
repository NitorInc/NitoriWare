using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashPlayerController : MonoBehaviour
{
    //On attack delegate, beat is null if attack is a miss
    public delegate void AttackDelegate(YoumuSlashBeatMap.TargetBeat beat);
    public delegate void GameplayEndDelegate();
    public static GameplayEndDelegate onGameplayEnd;
    public static AttackDelegate onAttack;

    public delegate void FaiLDelegate();
    public static FaiLDelegate onFail;

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
    private Animator[] lifeIndicators;
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
    private string inputStartCommand = "Go!";
    [SerializeField]
    private bool autoSlash;
    public bool AutoSlash
    {
        get { return autoSlash; }
        set { autoSlash = value; }
    }

    [SerializeField]
    private int health = 3;
    [SerializeField]
    private int postMissBeatCooldown = 2;
    [SerializeField]
    private bool emptySwingsDepleteHealth = true;
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
    [SerializeField]
    private float voicePan;
    
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
    AudioSource sfxSource;
    bool failQueued = false;
    int nextMissableBeat = 0;
    float finalGameplayBeat;
    bool gameplayComplete = false;

    private void Awake()
    {
        onAttack = null;
        onFail = null;
        onGameplayEnd = null;
        sfxSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        YoumuSlashTimingController.onBeat += onBeat;
        YoumuSlashTargetSpawner.OnTargetLaunch += onTargetLaunched;
        nextTarget = getFirstActiveTarget();
        finalGameplayBeat = timingData.BeatMap.TargetBeats.Last().HitBeat;
    }

    void onTargetLaunched(YoumuSlashBeatMap.TargetBeat target)
    {
        if (autoSlash)
            Invoke("performAutoSlash", (target.HitBeat - timingData.CurrentBeat) * timingData.BeatDuration);
        
        if ((!firstTargetStareMode || timingData.BeatMap.getFirstActiveTarget(timingData.CurrentBeat, hitTimeFudge.y) == target)
            && !attacking
            && getFirstHittableTarget(YoumuSlashBeatMap.TargetBeat.Direction.Any) == null)
            rigAnimator.SetBool("LookBack", isFacingRight() != (target.HitDirection == YoumuSlashBeatMap.TargetBeat.Direction.Right));
        if (target.HitEffect.ToString().EndsWith("Burst"))
        {
            rigAnimator.ResetTrigger("UnSquint");
            rigAnimator.SetTrigger("Squint");
            rigAnimator.SetBool("ForceTense", true);
            Invoke("unSquint", timingData.BeatDuration * 4f);
        }
    }

    void unSquint()
    {
        rigAnimator.ResetTrigger("Squint");
        rigAnimator.SetTrigger("UnSquint");
        rigAnimator.SetBool("ForceTense", false);
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

    void checkForGameplayEnd(YoumuSlashBeatMap.TargetBeat attackedBeat = null)
    {
        if (attackedBeat != null && attackedBeat.HitBeat >= finalGameplayBeat)
            gameplayComplete = true;
        else if (timingData.CurrentBeat >= finalGameplayBeat + hitTimeFudge.y)
            gameplayComplete = true;

        if (gameplayComplete)
        {
            AllowInput = false;
            onGameplayEnd();
        }
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
        checkForGameplayEnd();
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
        MicrogameController.instance.displayLocalizedCommand("commandb", inputStartCommand);
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

    public bool getBobEnabled() => rigAnimator.GetBool("EnableBob");

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

    void queueFail()
    {
        failQueued = true;
    }

    void fail()
    {
        enabled = false;
        YoumuSlashTimingController.onBeat = null;
        CancelInvoke();

        rigAnimator.SetBool("Fail", true);
        if (attacking)
            returnToIdle();

        onFail.Invoke();
        MicrogameController.instance.setVictory(false);
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
        if (allowInput && !failQueued)
            handleInput();
        
        var currentNextTarget = getFirstActiveTarget();
        if (nextTarget != currentNextTarget)
        {
            if (nextTarget != null && !nextTarget.slashed)
            {
                triggerMiss();
                if (failQueued)
                    fail();
                else if (canReactToMissedNote())
                    playNoteMissReaction();
                else
                    noteMissReactionQueued = true;
            }
            nextTarget = currentNextTarget;
            checkForGameplayEnd();
        }
        else if (canReactToMissedNote())
        {
            if (failQueued)
                fail();
            else if (noteMissReactionQueued)
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

        //From here below slash attempt is confirmed
        attackWasSuccess = isHit;
        slashCooldownTimer = slashCooldown;
        noteMissReactionQueued = false;
        lastAttackTime = Time.time;
        if (!(onAttack == null))
            onAttack(hitTarget);
        if (!attackWasSuccess && emptySwingsDepleteHealth)
            noteMissReactionQueued = true;

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
            triggerMiss(emptySwingsDepleteHealth);

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


        rigAnimator.SetBool("Scream", false);
        holdAttack = false;
        if (isHit)
        {
            //Hit successful
            var offset = AutoSlash ? 0f
                : timingData.CurrentBeat - hitTarget.HitBeat;
            hitTarget.launchInstance.slash(MathHelper.randomRangeFromVector(sliceAngleRange), slashAnimationEffectTime, offset);
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
                    playSfx(screamClip, direction, false);
                    holdAttack = true;
                    break;
                default:
                    playSfx(hitVoiceClip, direction, true);
                    break;
            }
            if (!reAttacking)
                spriteTrail.resetTrail(spriteTrailStartOffset * facingDirection, offset);

            checkForGameplayEnd(hitTarget);
        }
        else
        {
            //Missed
            playSfx(hitVoiceClip, direction, true);
            spriteTrail.resetTrail(spriteTrailStartOffset * facingDirection, 0f);
        }

        spriteTrail.EnableSpawn = isHit ? (!reAttacking) : false;
    }

    void triggerMiss(bool depleteHealth = true)
    {
        if (failQueued)
            return;

        if (depleteHealth && (timingData.CurrentBeat >= nextMissableBeat) && 
            !(MicrogameController.instance.isDebugMode() && Input.GetKey(KeyCode.S)))
        { 
            health--;
            nextMissableBeat = (int)Mathf.Ceil(timingData.CurrentBeat + postMissBeatCooldown);
            lifeIndicators[health].SetTrigger("Miss");
        }

        if (health <= 0)
        {
            if (attacking)
                queueFail();
            else
                fail();
        }
        else
        {
            upsetResetHits = upsetResetHitCount;
            rigAnimator.SetBool("Upset", true);
        }
    }

    void playSfx(AudioClip clip, YoumuSlashBeatMap.TargetBeat.Direction direction, bool varyPitch)
    {
        sfxSource.panStereo = voicePan *
            (direction == YoumuSlashBeatMap.TargetBeat.Direction.Right ? 1f : -1f);
        sfxSource.pitch = (varyPitch ? Random.Range(.95f, 1.05f) : 1f)
            * (gameplayComplete ? 1f : Time.timeScale);
        sfxSource.PlayOneShot(clip);
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