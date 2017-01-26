using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShuffleController : MonoBehaviour
{
	public int cupCount, shuffleCount;
	public float shuffleTime, shuffleStartDelay;
	public GameObject cupPrefab;

	private CupShuffleCup[] cups;

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
		Waiting,
		Victory,
		Loss
	}

	void Start ()
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
		MicrogameController.instance.displayCommand("Choose!");
		state = State.Waiting;
	}

	void setStatus()
	{

	}

	void shuffle()
	{
		int indexA = Random.Range(0, cupCount), indexB;
		do {indexB = Random.Range(0, cupCount);} while (indexA == indexB);
		bool cupAUp = Random.value < .5f;
		CupShuffleCup cupA = cups[indexA], cupB = cups[indexB];

		cupA.endAnimation();
		cupB.endAnimation();
		cupA.startAnimation(cupB.position - cupA.position, shuffleTime, cupAUp);
		cupB.startAnimation(cupA.position - cupB.position, shuffleTime, !cupAUp);
	}

	void Update ()
	{

	}
}
