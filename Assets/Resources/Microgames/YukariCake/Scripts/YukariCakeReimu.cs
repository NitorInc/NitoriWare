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
        Debug.Log(string.Format("We're going to change state at {0}", MicrogameTimer.instance.beatsLeft));
        Animator.SetTrigger("changeState");
    }

    public void Stop()
    {
        isActive = false;
    }

    public void SetStateChangeAt()
    {
        float beatsLeft = MicrogameTimer.instance.beatsLeft;
        float duration = 0f;

        if (IsAlert)
            Debug.Log("<< ALERT -> NONALERT >>");
        else
            Debug.Log("<< NONALERT -> ALERT >>");

        StateChangeAt = Mathf.Clamp(beatsLeft - duration, 0.0f, 8f);
        //StateChangeAt = beatsLeft - duration;
        Debug.Log(StateChangeAt);
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
        return new Queue<float>(new[]{ 6f, 4.5f });
    }

    public Queue<float> DifficultyMediumPattern()
    {
        // Medium is intended to be a bit tougher.
        return new Queue<float>(new[]{ 7f, Random.Range(5f, 6f), Random.Range(3.5f, 4f), 2f });
    }

    public Queue<float> DifficultyHardPattern()
    {
        // Hard is supposed to be more sporadic.
        var queue = new Queue<float>();
        var beatsLeft = 8f - Random.Range(0.1f, 0.75f);
        queue.Enqueue(beatsLeft);

        var fromAlertToNonalert = true;
        while(beatsLeft > 0)
        {
            // Implies that Reimu is alert for shorter periods of time.
            beatsLeft -= (fromAlertToNonalert) ? Random.Range(0.5f, 1.8f) : Random.Range(2f, 3.25f);
            if (beatsLeft > 0)
                queue.Enqueue(beatsLeft);
        }

        return queue;
    }

    #endregion


}
