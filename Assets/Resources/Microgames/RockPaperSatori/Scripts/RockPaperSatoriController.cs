using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperSatoriController : MonoBehaviour
{
    [SerializeField]
    private float startDelay;

    [SerializeField]
    private RockPaperSatoriPlayer player;
    [SerializeField]
    private RockPaperSatoriOpponent opponent;
    
    public enum Move
    {
        Rock,
        Paper,
        Scissors
    }

    void Start()
    {
        Invoke("startGame", startDelay);
    }

    void startGame()
    {
        player.startGame();
        opponent.startGame();
    }
}
