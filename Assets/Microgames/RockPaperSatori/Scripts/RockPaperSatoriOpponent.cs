using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperSatoriOpponent : MonoBehaviour
{
    [SerializeField]
    private RockPaperSatoriController.Move move;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private GameObject thoughtBubble;

    [SerializeField]
    private SpriteRenderer handRenderer;
    [SerializeField]
    private SpriteRenderer thoughtRenderer;
    [SerializeField]
    private Sprite[] moveThoughts;
    [SerializeField]
    private AudioClip shuffleClip;

    [Header("Character sprites")]
    [SerializeField]
    private Sprite[] moveHands;
    [SerializeField]
    private Sprite[] fakeThoughts;
    
    [Header("Timing fields")]
    [SerializeField]
    private Vector2 fakeShuffleAmountRange;
    [SerializeField]
    private float fakeShuffleDuration;
    [Header("Set to high number to stay on first real hand image")]
    [SerializeField]
    private float realShuffleDuration;


    
	void Start ()
    {
        move = (RockPaperSatoriController.Move)Random.Range(0, 3);
	}

    public void startGame()
    {
        //TODO make thought bubble appear?
        rigAnimator.enabled = true;
        invokeShuffleRound();
    }

    public void throwHand(RockPaperSatoriPlayer.State playerState)
    {
        thoughtBubble.SetActive(false);
        rigAnimator.SetInteger("State", (int)playerState);
        handRenderer.sprite = moveHands[(int)move];
        thoughtRenderer.sprite = null;
        CancelInvoke();
    }
	
	void invokeShuffleRound()
    {
        thoughtBubble.SetActive(true);

        //Fake shufles
        int fakeShuffleAmount = Random.Range((int)fakeShuffleAmountRange.x, (int)fakeShuffleAmountRange.y + 1);
        for (int i = 0; i < fakeShuffleAmount; i++)
        {
            Invoke("fakeShuffle", (float)i * fakeShuffleDuration);
        }

        //Real shuffle
        Invoke("realShuffle", (float)fakeShuffleAmount * fakeShuffleDuration);

        //Recusively invoke after real shuffle
        //Invoke("invokeShuffleRound", ((float)fakeShuffleAmount * fakeShuffleDuration) + realShuffleDuration);

    }

    void fakeShuffle()
    {
        //Shuffle until we get a different sprite
        var currentSprite = thoughtRenderer.sprite;
        do
        {
            thoughtRenderer.sprite = fakeThoughts[Random.Range(0, fakeThoughts.Length)];
        }
        while (thoughtRenderer.sprite == currentSprite || fakeThoughts.Length < 2);
        playShuffleSound();
    }

    void realShuffle()
    {
        thoughtRenderer.sprite = moveThoughts[(int)move];
        playShuffleSound();
    }

    void playShuffleSound()
    {
        MicrogameController.instance.playSFX(shuffleClip);
    }

    public RockPaperSatoriController.Move getMove()
    {
        return move;
    }
}
