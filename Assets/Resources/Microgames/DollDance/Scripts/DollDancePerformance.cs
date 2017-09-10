using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDancePerformance : MonoBehaviour
{

    DollDanceSequence sequence;

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
        foreach (DollDanceSequence.Move move in moves)
        {
            this.animator.Play(move.ToString(), 1);
            yield return new WaitForSeconds(this.animator.GetCurrentAnimatorStateInfo(1).length);
        }

        this.OnPreviewComplete();
    }
    
    void OnPreviewComplete()
    {
        // Start listening for sequence input
        this.sequenceListener = this.gameObject.AddComponent<DollDanceSequenceListener>();
    }
    
    public void Perform(DollDanceSequence.Move move)
    {
        this.animator.SetTrigger(move.ToString());
    }

    public void Release(DollDanceSequence.Move move)
    {
        if (this.animator.GetCurrentAnimatorStateInfo(2).IsName(move.ToString()))
            this.animator.SetTrigger(DollDanceSequence.Move.Idle.ToString());
    }

    public void StartSequence(DollDanceSequence sequence)
    {
        this.sequence = sequence;

        this.StartCoroutine(RunPreview(this.sequence.CopySequence()));
    }

    public void Succeed()
    {
        Destroy(this.sequenceListener);

        controller.Victory();
    }

    public void Fail()
    {
        Destroy(this.sequenceListener);

        controller.Defeat();
    }

    public DollDanceSequence GetSequence()
    {
        return this.sequence;
    }

}
