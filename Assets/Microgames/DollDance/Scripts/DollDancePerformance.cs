using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDancePerformance : MonoBehaviour
{

    // Important layers on the animator
    const int POINT_LAYER = 1;
    const int EYE_LAYER = 2;

    DollDanceSequence sequence;

    [Header("Timing for sequences in beats")]
    public float cueStartDelay = 1f;
    public float cuePointDelay = 1f;
    public float cueSettleDelay = 1f;
    public float dollInputStartDelay = .5f;
    public float dollResultVictoryDelay = 1f;
    public float dollResultFailDelay = 1f;
    public float aliceUnshadeDelay = 1f;


    [Header("Sound effects")]
    [SerializeField]
    AudioClip pointClip;
    [SerializeField]
    AudioClip victoryClip;
    [SerializeField]
    AudioClip failClip;
    [SerializeField]
    AudioClip dollClip;

    [Header("Sound sources")]
    [SerializeField]
    AudioSource pointSource;
    [SerializeField]
    AudioSource dollSource;

    [Header("Result effects")]
    [SerializeField]
    GameObject roseEffect;
    
    [Header("Color shade scripts for each character")]
    [SerializeField]
    public DollDanceColorShade aliceShadeComponent;
    [SerializeField]
    public DollDanceColorShade dollShadeComponent;

    [Header("Additional animators")]
    [SerializeField]
    private Animator[] eyeAnimators;

    Animator animator;
    DollDanceController controller;
    DollDanceSequenceListener sequenceListener;

    Dictionary<DollDanceSequence.Move, float> pitchMap;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = FindObjectOfType<DollDanceController>();

        cueStartDelay *= (float)Microgame.BeatLength;
        cuePointDelay *= (float)Microgame.BeatLength;
        cueSettleDelay *= (float)Microgame.BeatLength;
        dollInputStartDelay *= (float)Microgame.BeatLength;
        dollResultVictoryDelay *= (float)Microgame.BeatLength;
        dollResultFailDelay *= (float)Microgame.BeatLength;
        aliceUnshadeDelay *= (float)Microgame.BeatLength;
    }

    void Start()
    {
        pitchMap = new Dictionary<DollDanceSequence.Move, float>()
        {
            { DollDanceSequence.Move.Up, 1F },
            { DollDanceSequence.Move.Down, 0.9F },
            { DollDanceSequence.Move.Left, 0.8F },
            { DollDanceSequence.Move.Right, 0.7F },
        };
    }

    IEnumerator RunPreview(List<DollDanceSequence.Move> moves)
    {
        // Play a preview of the sequence so the player knows what to copy
        animator.SetBool("Preview", true);

        // Pointing delay
        yield return new WaitForSeconds(cueStartDelay);// + MicrogameTimer.instance.beatsLeft);

        //dollShadeComponent.setShaded(true);

        for (int i = 0; i < moves.Count; i++)
        {
            Point(moves[i]);

            //Delay until next point, unless this is the final point
            if (i < moves.Count - 1)
                yield return new WaitForSeconds(cuePointDelay);
        }

        yield return new WaitForSeconds(cueSettleDelay);

        animator.Play("Settle");
        //animator.Play("Forward", EYE_LAYER);
        playEyeAnimation("LookDoll");

        // Delay until user input is allowed
        yield return new WaitForSeconds(dollInputStartDelay);


        //aliceShadeComponent.setShaded(true);
        //dollShadeComponent.setShaded(false);

        animator.Play("GetReady");

        animator.SetBool("Preview", false);
        OnPreviewComplete();
    }

    IEnumerator VictoryAnimation()
    {
        // Make the doll transition to a victory pose
        animator.SetBool("Succeed", true);

        //Play cheering before delay
        MicrogameController.instance.playSFX(victoryClip);

        // After a short delay, play appropriate animations
        yield return new WaitForSeconds(dollResultVictoryDelay);
        animator.Play("ThumbsUp");
        playEyeAnimation("LookBack");

        //Start rose effect
        roseEffect.SetActive(true);
        //aliceShadeComponent.setShaded(false);
    }

    IEnumerator FailAnimation()
    {
        //Wait for failure delay
        yield return new WaitForSeconds(dollResultFailDelay);

        //Do failure animations
        animator.SetBool("Fail", true);
        animator.Play("Frown");
        animator.Play("ThumbsDown");
        playEyeAnimation("LookBack");

        //Play failure sound
        MicrogameController.instance.playSFX(failClip);

        //Set Alice to unshaded
        //aliceShadeComponent.setShaded(false);
    }

    void Point(DollDanceSequence.Move move)
    {
        // Point, look, play a sound
        animator.Play(move.ToString(), POINT_LAYER);
        //animator.Play(move.ToString(), EYE_LAYER);

        playEyeAnimation(move.ToString());

        pointSource.pitch = pitchMap[move];
        pointSource.PlayOneShot(pointClip);
    }

    void OnPreviewComplete()
    {
        // Start listening for sequence input
        sequenceListener = gameObject.AddComponent<DollDanceSequenceListener>();
    }

    public void StartSequence(DollDanceSequence sequence)
    {
        // Initiate a new performance based on a given sequence
        this.sequence = sequence;
        StartCoroutine(RunPreview(sequence.CopySequence()));
    }

    public void Perform(DollDanceSequence.Move move)
    {
        // Doll does a move
        animator.SetTrigger(move.ToString());

        dollSource.Stop();
        dollSource.pitch = pitchMap[move];
        dollSource.PlayOneShot(dollClip);
    }

    public void Succeed()
    {
        // Stop listening for sequence input
        Destroy(sequenceListener);

        // Do victory animations
        StartCoroutine(VictoryAnimation());

        // Tell the game that we succeeded
        controller.Victory();
    }

    public void Fail()
    {
        // Stop listening for sequence input
        Destroy(sequenceListener);
        
        StartCoroutine(FailAnimation());

        // Tell the game that we failed
        controller.Defeat();
    }

    void playEyeAnimation(string animation)
    {
        foreach (Animator eyeAnimator in eyeAnimators)
        {
            eyeAnimator.Play(animation);
        }
    }

    public DollDanceSequence GetSequence()
    {
        return sequence;
    }

    public DollDanceColorShade getAliceShadeComponent()
    {
        return aliceShadeComponent;
    }

    public DollDanceColorShade getDollShadeComponent()
    {
        return aliceShadeComponent;
    }

}
