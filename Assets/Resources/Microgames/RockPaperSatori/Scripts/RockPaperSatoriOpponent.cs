using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperSatoriOpponent : MonoBehaviour
{
    [SerializeField]
    private RockPaperSatoriRNGController.Move move;

    [Header("Character sprites")]
    [SerializeField]
    private SpriteRenderer handRenderer;
    [SerializeField]
    private Sprite[] moveHands;
    [SerializeField]
    private SpriteRenderer thoughtRenderer;
    [SerializeField]
    private Sprite[] moveThoughts;
    [SerializeField]
    private Sprite[] fakeThoughts;
    
    [Header("Timing fields")]
    [SerializeField]
    private float startDelay;
    [SerializeField]
    private Vector2 fakeShuffleAmountRange;
    [SerializeField]
    private float fakeShuffleDuration;
    [Header("Set to high number to stay on first real hand image")]
    [SerializeField]
    private float realShuffleDuration;
    
	void Start ()
    {
        move = (RockPaperSatoriRNGController.Move)Random.Range(0, 3);
        Invoke("startGame", startDelay);
	}

    void startGame()
    {
        //TODO make thought bubble appear?
        invokeShuffleRound();
    }
	
	void invokeShuffleRound()
    {
        //Fake shufles
        int fakeShuffleAmount = Random.Range((int)fakeShuffleAmountRange.x, (int)fakeShuffleAmountRange.y + 1);
        for (int i = 0; i < fakeShuffleAmount; i++)
        {
            Invoke("fakeShuffle", (float)i * fakeShuffleDuration);
        }

        //Real shuffle
        Invoke("realShuffle", (float)fakeShuffleAmount * fakeShuffleDuration);

        //Recusively invoke after real shuffle
        Invoke("invokeShuffleRound", ((float)fakeShuffleAmount * fakeShuffleDuration) + realShuffleDuration);

    }

    void fakeShuffle()
    {
        //Shuffle untile we get a different sprite
        var currentSprite = thoughtRenderer.sprite;
        do
        {
            thoughtRenderer.sprite = fakeThoughts[Random.Range(0, fakeThoughts.Length)];
        }
        while (thoughtRenderer.sprite == currentSprite || fakeThoughts.Length < 2);
    }

    void realShuffle()
    {
        thoughtRenderer.sprite = moveThoughts[(int)move];
    }

    public RockPaperSatoriRNGController.Move getMove()
    {
        return move;
    }
}
