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

    public void startGame()
    {
        rigAnimator.enabled = true;
    }

    public void throwHand(RockPaperSatoriController.Move move, bool victory)
    {
        rigAnimator.SetInteger("State", victory ? 1 : 2);
        handRenderer.sprite = moveHands[(int)move];
    }
}
