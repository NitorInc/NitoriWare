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

    [Header("Sound made when doll moves")]
    [SerializeField]
    AudioClip dollClip;

    [Header("Victory particles")]
    [SerializeField]
    GameObject roseEffect;
    //ParticleSystem[] roseParticles;
    
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
        this.animator = this.GetComponentInChildren<Animator>();
        this.controller = FindObjectOfType<DollDanceController>();

        cueStartDelay *= StageController.beatLength;
        cuePointDelay *= StageController.beatLength;
        cueSettleDelay *= StageController.beatLength;
        dollInputStartDelay *= StageController.beatLength;
        dollResultVictoryDelay *= StageController.beatLength;
        dollResultFailDelay *= StageController.beatLength;
        aliceUnshadeDelay *= StageController.beatLength;
    }

    void Start()
    {
        this.pitchMap = new Dictionary<DollDanceSequence.Move, float>()
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
        this.animator.SetBool("Preview", true);

        // Pointing delay
        yield return new WaitForSeconds(cueStartDelay);// + MicrogameTimer.instance.beatsLeft);

        dollShadeComponent.setShaded(true);

        for (int i = 0; i < moves.Count; i++)
        {
            this.Point(moves[i]);

            //Delay until next point, unless this is the final point
            if (i < moves.Count - 1)
                yield return new WaitForSeconds(cuePointDelay);
        }

        yield return new WaitForSeconds(cueSettleDelay);

        this.animator.Play("Settle");
        this.animator.Play("Forward", EYE_LAYER);

        // Delay until user input is allowed
        yield return new WaitForSeconds(dollInputStartDelay);


        aliceShadeComponent.setShaded(true);
        dollShadeComponent.setShaded(false);

        this.animator.Play("GetReady");

        this.animator.SetBool("Preview", false);
        this.OnPreviewComplete();
    }

    IEnumerator VictoryAnimation()
    {
        // Make the doll transition to a victory pose
        this.animator.SetBool("Succeed", true);

        MicrogameController.instance.playSFX(victoryClip);

        // After a short delay, give a thumbs up
        yield return new WaitForSeconds(dollResultVictoryDelay);
        this.animator.Play("ThumbsUp");
        roseEffect.SetActive(true);
        aliceShadeComponent.setShaded(false);
    }

    void Point(DollDanceSequence.Move move)
    {
        // Point, look, play a sound
        this.animator.Play(move.ToString(), POINT_LAYER);
        this.animator.Play(move.ToString(), EYE_LAYER);

        MicrogameController.instance.playSFX(this.pointClip, pitchMult: this.pitchMap[move] + .15f);
    }

    void OnPreviewComplete()
    {
        // Start listening for sequence input
        this.sequenceListener = this.gameObject.AddComponent<DollDanceSequenceListener>();
    }

    public void StartSequence(DollDanceSequence sequence)
    {
        // Initiate a new performance based on a given sequence
        this.sequence = sequence;
        this.StartCoroutine(RunPreview(this.sequence.CopySequence()));
    }

    public void Perform(DollDanceSequence.Move move)
    {
        // Doll does a move
        this.animator.SetTrigger(move.ToString());

        MicrogameController.instance.playSFX(this.dollClip, pitchMult: this.pitchMap[move]);
    }

    public void Succeed()
    {
        // Stop listening for sequence input
        Destroy(this.sequenceListener);

        // Do victory animations
        this.StartCoroutine(this.VictoryAnimation());

        // Tell the game that we succeeded
        controller.Victory();
    }

    public void Fail()
    {
        // Stop listening for sequence input
        Destroy(this.sequenceListener);
        
        this.StartCoroutine(this.FailAnimation());

        // Tell the game that we failed
        controller.Defeat();
    }

    IEnumerator FailAnimation()
    {
        // Do failure animations
        yield return new WaitForSeconds(dollResultFailDelay);
        this.animator.SetBool("Fail", true);
        this.animator.Play("Frown");
        this.animator.Play("ThumbsDown");
        MicrogameController.instance.playSFX(failClip);
        aliceShadeComponent.setShaded(false);
    }

    public DollDanceSequence GetSequence()
    {
        return this.sequence;
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
