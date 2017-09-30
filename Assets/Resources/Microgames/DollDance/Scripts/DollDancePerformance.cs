using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDancePerformance : MonoBehaviour
{

    // Important layers on the animator
    const int POINT_LAYER = 1;
    const int EYE_LAYER = 2;

    DollDanceSequence sequence;

    [Header("Sound made when pointing")]
    [SerializeField]
    AudioSource pointSound;

    [Header("Sound made when doll moves")]
    [SerializeField]
    AudioSource dollSound;

    Animator animator;
    DollDanceController controller;
    DollDanceSequenceListener sequenceListener;
    
    void Awake()
    {
        this.animator = this.GetComponentInChildren<Animator>();
        this.controller = FindObjectOfType<DollDanceController>();
    }
    
    IEnumerator RunPreview(List<DollDanceSequence.Move> moves)
    {
        // Play a preview of the sequence so the player knows what to copy
        this.animator.SetBool("Preview", true);
        float initialDelay = StageController.beatLength;
        float pointDelay = StageController.beatLength;

        // Pointing delay
        yield return new WaitForSeconds(initialDelay);

        foreach (DollDanceSequence.Move move in moves)
        {
            this.Point(move);

            // Delay until next point
            yield return new WaitForSeconds(pointDelay);
        }

        this.animator.Play("Settle");
        this.animator.Play("Forward", EYE_LAYER);

        // Delay until user input is allowed
        yield return new WaitForSeconds(pointDelay);

        this.animator.Play("GetReady");

        this.animator.SetBool("Preview", false);
        this.OnPreviewComplete();
    }

    IEnumerator VictoryAnimation()
    {
        // Make the doll transition to a victory pose
        this.animator.SetBool("Succeed", true);

        // After a short delay, give a thumbs up
        yield return new WaitForSeconds(StageController.beatLength);
        this.animator.Play("ThumbsUp");
    }

    void Point(DollDanceSequence.Move move)
    {
        // Point, look, play a sound
        this.animator.Play(move.ToString(), POINT_LAYER);
        this.animator.Play(move.ToString(), EYE_LAYER);
        this.pointSound.Play();
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
        this.dollSound.Play();
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

        // Do failure animations
        this.animator.SetBool("Fail", true);
        this.animator.Play("Frown");
        this.animator.Play("ThumbsDown");

        // Tell the game that we failed
        controller.Defeat();
    }

    public DollDanceSequence GetSequence()
    {
        return this.sequence;
    }

}
