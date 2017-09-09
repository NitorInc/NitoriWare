using System.Collections.Generic;
using UnityEngine;

public class DollDancePerformance : MonoBehaviour
{

    DollDanceSequence sequence;

    Animator animator;
    DollDanceController controller;

    void Awake()
    {
        this.animator = this.GetComponentInChildren<Animator>();
        this.controller = FindObjectOfType<DollDanceController>();
    }

    void PreviewSequence()
    {
        List<DollDanceSequence.Move> moves = this.sequence.CopySequence();
        foreach (DollDanceSequence.Move move in moves)
        {
            print(move);
        }

        OnPreviewComplete();
    }

    void OnPreviewComplete()
    {
        // Start listening for sequence input
        this.gameObject.AddComponent<DollDanceSequenceListener>();
    }

    public void Perform(DollDanceSequence.Move move)
    {
        animator.Play(move.ToString());
    }

    public void Succeed()
    {
        controller.Victory();
    }

    public void Fail()
    {
        controller.Defeat();
    }

    public void StartSequence(DollDanceSequence sequence)
    {
        this.sequence = sequence;

        this.PreviewSequence();
    }

    public DollDanceSequence GetSequence()
    {
        return this.sequence;
    }

}
