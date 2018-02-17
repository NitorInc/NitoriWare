using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperSatoriPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator rigAnimator;

    [Header("Character sprites")]
    [SerializeField]
    private SpriteRenderer handRenderer;
    [SerializeField]
    private Sprite[] moveHands;

    public enum State
    {
        Default,
        Victory,
        Loss,
        Tie
    }

    public void startGame()
    {
        rigAnimator.enabled = true;
    }

    public void throwHand(RockPaperSatoriController.Move move, State state)
    {
        rigAnimator.SetInteger("State", (int)state);
        handRenderer.sprite = moveHands[(int)move];
    }
}
