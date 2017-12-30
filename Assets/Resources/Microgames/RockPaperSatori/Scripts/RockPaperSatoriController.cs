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
    

    bool handleTie()
    {
        return false;
    }
}
