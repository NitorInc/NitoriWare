using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBubble_SpeechBubble : MonoBehaviour {

    private GameObject targetCharacter;

    private GameObject indicator;

    private Animator targetMouthAnimator;

    private AdvancingText textObject;

    private float bubbleProgress;       

    [SerializeField]
    private Vector2 castOffset;                     // Offset of the box cast that interacts with the target

    [SerializeField]
    private float castWidth;                        // Widht of the box cast that interacts with the target

    [SerializeField]
    private float castHeight;                       // Height of the box cast that interacts with the target

    [SerializeField]
    private float textSpeed;                        // Speed in which the text appears

    private int counterLimitSpeedUpdate = 100;      // How many times I should update the speed?
    private int counterSpeedUpdate;                 // Times I've updated the speed
    
    private float refSpeed;                         // Reference to calculate the text speed
    private TMPro.TMP_Text textComponent;           // Reference to the text object component


    [SerializeField]                                // Sprite to switch to upon finishing
    private Sprite finishedSprite;

    [SerializeField]
    private AudioClip[] chirpClips;
    [SerializeField]
    private float chirpRepeatTime;
    [SerializeField]
    private Vector2 chirpPitchRange;
    [SerializeField]
    private float chirpVolume = 1f;


    private int CLOSED_MOUTH_PARAM = 0;
    private int SPEAKING_MOUTH_PARAM = 1;
    private int OPEN_MOUTH_PARAM = 2;

    private SpriteRenderer spriteRenderer;
    private Sprite initialSprite;
    private float lastChirpTime;
    private bool finishedTalking;


    // Use this for initialization
    void Start () {

        textObject = GetComponentInChildren<AdvancingText>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSprite = spriteRenderer.sprite;

        bubbleProgress = 0;
        lastChirpTime = 0f;
        counterSpeedUpdate = 0;

        stopSpeechText();

    }

    // Update is called once per frame
    void Update () {

        if (counterSpeedUpdate < counterLimitSpeedUpdate) {
            updateTextSpeed();
        }


        if (textSpeed > 0)
        {

            RaycastHit2D[] result = Physics2D.BoxCastAll(castOffset + (Vector2)transform.position, new Vector3(castWidth, castHeight, 1), 0, Vector2.zero);

            if (result.Length > 0)
            {
                foreach (RaycastHit2D r in result)
                {
                    if (r.collider.gameObject == targetCharacter)
                    {
                        advanceSpeechText();
                        updateBubbleProgress();
                        updateChirp();
                    }
                }
            }

            else
            {
                stopSpeechText();
            }

        }

        if (getBubbleProgress() == 100)
        {
            textObject.setAdvanceSpeed(0);
            setMouthAnimationParam(OPEN_MOUTH_PARAM);
        }

	}


    // Get bubble progress
    public float getBubbleProgress()
    {
        return bubbleProgress;
    }


    // Update bubble progress
    void updateBubbleProgress()
    {

        int totalChars = textObject.getTotalVisibleChars();
        int showChars = textObject.getVisibleChars();
        bubbleProgress = (showChars * 100)/ totalChars;
    }

    void updateChirp()
    {
        if (finishedTalking)
            return;
        if (Time.time - lastChirpTime >= chirpRepeatTime)
        {
            lastChirpTime = Time.time;
            MicrogameController.instance.playSFX(chirpClips[Random.Range(0, chirpClips.Length)],
                panStereo: AudioHelper.getAudioPan(transform.position.x),
                pitchMult: MathHelper.randomRangeFromVector(chirpPitchRange),
                volume: chirpVolume);
        }
    }


    // Activate target mouth animation
    void setMouthAnimationParam(int param)
    {
        if (targetMouthAnimator != null)
        {
            if (param == CLOSED_MOUTH_PARAM ||
                param == SPEAKING_MOUTH_PARAM ||
                param == OPEN_MOUTH_PARAM)
            {
                targetMouthAnimator.SetInteger("AnimateMouth", param);
            }
        }
    }


    // To set the speed in which the text will appear
    public void setTextSpeed(float speed)
    {
        refSpeed = speed;
        textComponent = GetComponentInChildren<AdvancingText>().GetComponent<TMPro.TMP_Text>();
        textSpeed = speed * textComponent.text.Length;
    }

    public void updateTextSpeed()
    {
        if (textComponent != null)
        {
            textSpeed = refSpeed * textComponent.text.Length;
            counterSpeedUpdate++;
        }
    }


    // To set the target of the speech bubble (and the animator)
    public void setTargetCharacter(GameObject target)
    {
        targetCharacter = target;
        if (target != null)
        {
            foreach (Animator anim in targetCharacter.GetComponentsInChildren<Animator>())
            {
                if(anim.gameObject != target)
                {
                    targetMouthAnimator = anim;
                }
            }
        }
    }

    public void setIndicator(GameObject indicator) => this.indicator = indicator;

    // Stop text from showing
    void stopSpeechText()
    {
        textObject.setAdvanceSpeed(0);
        setMouthAnimationParam(CLOSED_MOUTH_PARAM);
        spriteRenderer.sprite = initialSprite;
    }

    public void onFinishedTalking() => finishedTalking = true;

    // Advance text
    void advanceSpeechText()
    {
        textObject.setAdvanceSpeed(textSpeed);
        setMouthAnimationParam(SPEAKING_MOUTH_PARAM);
        spriteRenderer.sprite = finishedSprite;
    }

    // This is for showing where the square box cast is being placed
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(castOffset + (Vector2)transform.position, new Vector3(castWidth, castHeight, 1));
    }

}
