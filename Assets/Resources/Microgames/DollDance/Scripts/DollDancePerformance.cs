using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDancePerformance : MonoBehaviour
{

    DollDanceSequence sequence;

    Animator animator;
    DollDanceController controller;
    DollDanceSequenceListener sequenceListener;

    DollDanceSequence.Move lastMove;

    void Awake()
    {
        this.animator = this.GetComponentInChildren<Animator>();
        this.controller = FindObjectOfType<DollDanceController>();
    }
    
    IEnumerator RunPreview(List<DollDanceSequence.Move> moves)
    {
        foreach (DollDanceSequence.Move move in moves)
        {
            print(move.ToString());
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
        animator.SetInteger("CurrentMove", (int)move);

        this.lastMove = move;
    }

    public void Release(DollDanceSequence.Move move)
    {
        if (move == this.lastMove)
            animator.SetInteger("CurrentMove", 0);
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
