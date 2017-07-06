using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// YukariCakeReimu.cs
/// The logic that facilitates Reimu's AI and movement...
/// </summary>

public class YukariCakeReimu : MonoBehaviour {

    public Animator Animator;
    public AudioSource Source;
    public bool IsAlert = false;
    public Queue<float> ChangeSequence;
    public float StateChangeAt = 0;
    public int Level = 1;
    public AudioClip WooshTo, WooshBack;

    // Reimu
    private bool isActive = true;

	// Use this for initialization
	void Start () {
        if (Level >= 3) ApplyHardModeReimu();
        SetChangeSequence();
	}
	
	// Update is called once per frame
	void Update () {
        if(isActive)
        {
            if (MicrogameTimer.instance.beatsLeft <= 0 || ChangeSequence.Count == 0)
                Stop();
            else if (MicrogameTimer.instance.beatsLeft <= ChangeSequence.Peek())
            {
                ChangeState();
                ChangeSequence.Dequeue();
            }
        }

	}

    #region Sounds and Fun!

    public void PlayWoosh(string woosh)
    {
        // Don't play sounds if Yukari won
        if (MicrogameController.instance.getVictory())
            return;

        // Because Animation Events don't have a boolean :(
        if (woosh == "WooshBack")
            Source.PlayOneShot(WooshBack);
        else
            Source.PlayOneShot(WooshTo);
    }

    public void PlayFailureAnimation()
    {
        Animator.Play("FailureState");
    }

    #endregion

    #region Game Logic

    public void ChangeState()
    {
        //Debug.Log(string.Format("We're going to change state at {0}", MicrogameTimer.instance.beatsLeft));
        Animator.SetTrigger("changeState");
    }

    public void Stop()
    {
        isActive = false;
    }

    public void ApplyHardModeReimu()
    {
        Animator.SetBool("levelThree", true);
    }

    public void SetChangeSequence()
    {
        if (Level <= 1)
            ChangeSequence = DifficultyEasyPattern();
        else if (Level == 2)
            ChangeSequence = DifficultyMediumPattern();
        else
            ChangeSequence = DifficultyHardPattern();
    }

    public Queue<float> DifficultyEasyPattern()
    {
        // Easy is a set pattern.
        return new Queue<float>(new[]{ 7.6f, 4.4f });
    }

    public Queue<float> DifficultyMediumPattern()
    {
        // Medium has Reimu flipping twice, but should give a good window of time
        // if you strike right after she returns back.

        var queue = new Queue<float>();
        var beatsLeft = 8f;
        for (int i = 0; i < 4; i++)
        {
            beatsLeft -= (i == 0) ? Random.Range(0.25f, 0.45f) :
                         (i == 1) ? Random.Range(2f, 2.8f) :
                         (i == 2) ? Random.Range(0.8f, 1.2f) :
                         Random.Range(2f, 2.5f);
            queue.Enqueue(beatsLeft);
        }
        return queue;
    }

    public Queue<float> DifficultyHardPattern()
    {
        // Hard should have tighter timing windows.
        // Like, super tight. ;)

        var queue = new Queue<float>();
        var beatsLeft = 8f;
        for (int i = 0; i < 4; i++)
        {
            beatsLeft -= (i == 0) ? Random.Range(0.1f, 0.30f) :
                         (i == 1) ? Random.Range(2f, 2.5f) :
                         (i == 2) ? 0.5f :
                         Random.Range(2f, 1.5f);
            queue.Enqueue(beatsLeft);
        }
        return queue;
    }

    #endregion


}
