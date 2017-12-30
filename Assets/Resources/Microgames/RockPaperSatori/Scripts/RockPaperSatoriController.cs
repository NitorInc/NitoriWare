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
        if (move == opponent.getMove())
            handleTie();
        else if (MathHelper.trueMod(((int)move - 1), 3) == (int)opponent.getMove())
            setVictory(true);
        else
            setVictory(false);
    }
    

    void handleTie()
    {
        setVictory(false);
    }

    void setVictory(bool victory)
    {
        MicrogameController.instance.setVictory(victory, true);
    }
}
