using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupShuffleController : MonoBehaviour
{
	public int cupCount, shuffleCount;
	public float shuffleTime;
	public GameObject cupPrefab;

	private CupShuffleCup[] cups;


	void Start ()
	{
		cups = new CupShuffleCup[cupCount];
		for (int i = 0; i < cupCount; i++)
		{
			cups[i] = GameObject.Instantiate(cupPrefab).GetComponent<CupShuffleCup>();
			cups[i].position = i;
		}

		for (int i = 0; i < shuffleCount; i++)
		{
			Invoke("shuffle", i * shuffleTime);
		}
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
