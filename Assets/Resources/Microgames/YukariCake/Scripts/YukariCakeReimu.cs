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
    public bool IsAlert = true;
    public float StateChangeAt = 0;
    public int Level = 1;
    public AudioClip WooshTo;
    public AudioClip WooshBack;

	// Use this for initialization
	void Start () {
        SetStateChangeAt();
	}
	
	// Update is called once per frame
	void Update () {
        if (MicrogameTimer.instance.beatsLeft <= StateChangeAt && MicrogameTimer.instance.beatsLeft > 0)
        {
            Animator.SetTrigger("changeState");
            SetStateChangeAt();
        }
	}

    public void SetStateChangeAt()
    {
        float beatsLeft = MicrogameTimer.instance.beatsLeft;
        float duration;
        if (IsAlert)
            duration = Random.Range(1f, 4f);
        else
            duration = Random.Range(2f, 4f);
        StateChangeAt = Mathf.Clamp(beatsLeft - duration, 0.0f, 16f);
    }

    public void ToggleAlert()
    {
        IsAlert = !IsAlert;
        Debug.Log(string.Format("Changed at {0}, IsAlert: {1}", MicrogameTimer.instance.beatsLeft, IsAlert));
        SetStateChangeAt();
    }

    public void PlayWoosh(int wooshValue)
    {
        if (wooshValue == 0)
            Source.PlayOneShot(WooshBack);
        else
            Source.PlayOneShot(WooshTo);
    }

    public void Stop()
    {
        StateChangeAt = -1f;
    }

}
