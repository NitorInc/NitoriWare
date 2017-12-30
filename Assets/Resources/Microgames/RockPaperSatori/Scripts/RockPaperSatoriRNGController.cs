using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperSatoriRNGController : MonoBehaviour
{
    public Move opponentMove;
    public enum Move
    {
        Rock,
        Paper,
        Scissors
    }
    
	void Start ()
    {
        opponentMove = (Move)Random.Range((int)Move.Rock, (int)Move.Scissors + 1);
	}
}
