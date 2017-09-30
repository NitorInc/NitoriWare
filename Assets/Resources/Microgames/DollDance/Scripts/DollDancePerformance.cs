using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDancePerformance : MonoBehaviour
{

    const int POINT_LAYER = 1;
    const int DOLL_LAYER = 2;
    
    DollDanceSequence sequence;

    [SerializeField]
    AudioSource whipSound;

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

        // Delay until user input is allowed
        yield return new WaitForSeconds(pointDelay);

        this.OnPreviewComplete();
    }

    void Point(DollDanceSequence.Move move)
    {
        this.animator.Play(move.ToString());
        this.whipSound.Play();
    }

    void OnPreviewComplete()
    {
        this.animator.SetBool("Preview", false);
        this.animator.Play("GetReady");

        // Start listening for sequence input
        this.sequenceListener = this.gameObject.AddComponent<DollDanceSequenceListener>();
    }

    public void Perform(DollDanceSequence.Move move)
    {
        this.animator.SetTrigger(move.ToString());
    }
    
    public void StartSequence(DollDanceSequence sequence)
    {
        this.sequence = sequence;

        this.StartCoroutine(RunPreview(this.sequence.CopySequence()));
        
        //this.OnPreviewComplete();
    }

    public void Succeed()
    {
        Destroy(this.sequenceListener);
        this.animator.SetBool("Succeed", true);
        this.animator.Play("ThumbsUp");

        controller.Victory();
    }

    public void Fail()
    {
        Destroy(this.sequenceListener);
        this.animator.SetBool("Fail", true);
        this.animator.Play("Frown");
        this.animator.Play("ThumbsDown");

        controller.Defeat();
    }

    public DollDanceSequence GetSequence()
    {
        return this.sequence;
    }

}
