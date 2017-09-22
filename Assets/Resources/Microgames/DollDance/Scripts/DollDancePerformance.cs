using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollDancePerformance : MonoBehaviour
{

    const int POINT_LAYER = 1;
    const int DOLL_LAYER = 2;

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
        this.animator.SetBool("Preview", true);
        float moveLength = this.animator.GetCurrentAnimatorStateInfo(POINT_LAYER).length;
        yield return new WaitForSeconds(moveLength);// this.animator.GetCurrentAnimatorStateInfo(POINT_LAYER).length);

        foreach (DollDanceSequence.Move move in moves)
        {
            this.animator.Play(move.ToString());
            yield return new WaitForSeconds(moveLength * 2);// this.animator.GetNextAnimatorStateInfo(POINT_LAYER).length);
        }

        this.animator.Play("Settle");
        yield return new WaitForSeconds(moveLength);
        this.OnPreviewComplete();
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

    public void Release(DollDanceSequence.Move move)
    {
        //if (this.animator.GetCurrentAnimatorStateInfo(DOLL_LAYER).IsName(move.ToString()))
        //{
        //    this.animator.SetTrigger(DollDanceSequence.Move.Idle.ToString());
        //}
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
