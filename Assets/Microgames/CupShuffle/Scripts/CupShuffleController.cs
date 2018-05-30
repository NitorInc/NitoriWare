using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShuffleController : MonoBehaviour
{
	public static CupShuffleController instance;

	public int cupCount, shuffleCount;
	public float shuffleTime, shuffleStartDelay;
	public bool enableBackwardsShuffles;
	public float fakeOutChance;
	public GameObject cupPrefab;
	public AudioClip shuffleClip, correctClip, incorrectClip;

	private CupShuffleCup[] cups;
	private AudioSource _audioSource;

	[SerializeField]
	private State _state;
	public State state
	{
		get { return _state; }
		set { _state = value; for (int i = 0; i < cups.Length; cups[i++].state = state);}
	}


	public enum State
	{
		Idle,
		Shuffling,
		Choose,
		Victory,
		Loss
	}

	void Awake()
	{
		instance = this;
		_audioSource = GetComponent<AudioSource>();
	}

	void Start()
	{
		int correctCup = Random.Range(0, cupCount);
		cups = new CupShuffleCup[cupCount];
		for (int i = 0; i < cupCount; i++)
		{
			cups[i] = GameObject.Instantiate(cupPrefab).GetComponent<CupShuffleCup>();
			cups[i].position = i;
			cups[i].isCorrect = i == correctCup;
		}

		StartCoroutine(playAnimations());
	}


	private IEnumerator playAnimations()
	{
		yield return new WaitForSeconds(shuffleStartDelay);
		state = State.Shuffling;
		for (int i = 0; i < shuffleCount; i++)
		{
			shuffle();
			yield return new WaitForSeconds(shuffleTime);
		}

		yield return new WaitForSeconds(shuffleTime / 3f);
		MicrogameController.instance.displayLocalizedCommand("commandb", "Choose!");
		state = State.Choose;
	}

	public void victory()
	{
		state = State.Victory;
		MicrogameController.instance.setVictory(true, true);
		_audioSource.PlayOneShot(correctClip);
	}

	public void failure()
	{
		state = State.Loss;
		MicrogameController.instance.setVictory(false, true);
		_audioSource.PlayOneShot(incorrectClip);
	}

	void shuffle()
	{
		int indexA = Random.Range(0, cupCount), indexB;
		do {indexB = Random.Range(0, cupCount);} while (indexA == indexB);
		bool cupAUp = Random.value < .5f,
			backwards = enableBackwardsShuffles ? MathHelper.randomBool() : false,
			fakeout = Random.value < fakeOutChance;
		CupShuffleCup cupA = cups[indexA], cupB = cups[indexB];

		cupA.endAnimation();
		cupB.endAnimation();
		cupA.startAnimation(cupB.position - cupA.position, shuffleTime, cupAUp, backwards, fakeout);
		cupB.startAnimation(cupA.position - cupB.position, shuffleTime, !cupAUp, backwards, fakeout);

		_audioSource.PlayOneShot(shuffleClip);
	}

	void Update ()
	{

	}
}
