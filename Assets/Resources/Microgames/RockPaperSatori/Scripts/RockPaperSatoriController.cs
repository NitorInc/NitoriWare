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

    private bool gameStarted;

    public enum Move
    {
        Rock,
        Paper,
        Scissors
    }

    void Start()
    {
        gameStarted = false;
        Invoke("startGame", startDelay);
    }

    void startGame()
    {
        player.startGame();
        opponent.startGame();
        gameStarted = true;
    }

    public void makeMove(Move move)
    {
        //Decide if player won
        bool playerVictory;
        if (move == opponent.getMove())
            playerVictory = handleTie();
        else if (MathHelper.trueMod(((int)move - 1), 3) == (int)opponent.getMove())
            playerVictory = true;
        else
            playerVictory = false;

        //Set victory
        MicrogameController.instance.setVictory(playerVictory, true);
        player.throwHand(move, playerVictory);
        opponent.throwHand(!playerVictory); //Opposite
    }

    public bool isGameStarted()
    {
        return gameStarted;
    }
    

    bool handleTie()
    {
        return false;
    }
}
