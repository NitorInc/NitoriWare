using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandContoller : MonoBehaviour
{
	public static RockBandContoller instance;

	public RockBandNote[] notes;
	public Animator[] animators;
	public RockBandLight[] lights;
	public AudioClip victoryClip, failureClip;
	public AudioClip[] noteHitClips;
    public RockBandNote lineIndicatorPrefab;
    public float indicatorMin = -4;
    public float indicatorMax = 10;

    private AudioSource _audioSource;
	private State state;
	public enum State
	{
		Default,
		Victory,
		Failure,
		Hit
	}

	void Awake()
	{
		instance = this;
		_audioSource = GetComponent<AudioSource>();
        QualitySettings.pixelLightCount = lights.Length;
	}

    private void Start()
    {
        for (float i = indicatorMin; i < indicatorMax; i += .5f)
        {
            var note = Instantiate(lineIndicatorPrefab, transform.position, Quaternion.identity);
            note.transform.parent = transform;
            note.targetBeat = i;
            if (i % 1 == .5)
            {
                var noteScale = note.transform.localScale;
                noteScale.Scale(new Vector3(.3f, 1f, 1f));
                note.transform.localScale = noteScale;
            }
        }
    }

    public void victory()
	{
		setState(State.Victory);
		MicrogameController.instance.setVictory(true, true);
		foreach (RockBandLight light in lights)
		{
			light.onVictory();
		}
		_audioSource.PlayOneShot(victoryClip);
	}

	public void failure()
	{
		setState(State.Failure);
		MicrogameController.instance.setVictory(false, true);

		for (int i = 0; i < notes.Length; i++)
		{
			notes[i].gameObject.SetActive(false);
		}
		foreach (RockBandLight light in lights)
		{
			Animator lightAnimator = light.GetComponentInChildren<Animator>();
			if (lightAnimator != null)
				lightAnimator.enabled = false;
		}
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
		_audioSource.PlayOneShot(failureClip);
	}

	void hitNote()
	{
		setState(State.Hit);
		foreach (RockBandLight light in lights)
		{
			light.onHit();
		}
		_audioSource.PlayOneShot(noteHitClips[Random.Range(0, noteHitClips.Length)]);
	}
	
	void Update ()
	{
		if ((state == State.Default || state == State.Hit) && MicrogameController.instance.session.BeatsRemaining < 7f)
			checkForInput();
        updateAnimators();
    }

	void checkForInput()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow)
			|| Input.GetKeyDown(KeyCode.UpArrow)
			|| Input.GetKeyDown(KeyCode.LeftArrow)
			|| Input.GetKeyDown(KeyCode.DownArrow))
		{
			for (int i = 0; i < notes.Length; i++)
			{
				if (notes[i].state == RockBandNote.State.InRange)
				{
					if (Input.GetKeyDown(notes[i].key))
					{
						notes[i].playNote();
						if (i == notes.Length - 1)
							victory();
						else
						{
							hitNote();
						}
					}
					else
					{
						failure();
					}
					return;
				}
			}
			failure();
		}
	}

    void updateAnimators()
    {
        foreach (Animator animator in animators)
        {
            animator.SetFloat("beatFraction", 1f - (MicrogameController.instance.session.BeatsRemaining % 1f));
        }
    }

	void setState(State state)
	{
		this.state = state;
        foreach (Animator animator in animators)
        {
            animator.SetInteger("state", (int)state);
        }

	}
}
