using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SumiWheelCardController : MonoBehaviour
{
    [SerializeField]
    private Animator sumiRig;
    [SerializeField]
    private Transform cardsParent;
    [SerializeField]
    private Transform thoughtsParent;
    [SerializeField]
    private AudioClip clickClip;
    [SerializeField]
    private AudioClip victoryClip;
    [SerializeField]
    private AudioClip lossClip;
    
    private List<SumiWheelThoughtCard> thoughtPool;
    
    void Start()
    {
        thoughtPool = thoughtsParent.GetComponentsInChildren<SumiWheelThoughtCard>().ToList();

        var cardPool = cardsParent.transform.GetComponentsInChildren<SumiWheelCardClick>().ToList();
        foreach (var card in cardPool)
        {
            card.setController(this);
        }
        foreach (var thought in thoughtPool)
        {
            var chosenCard = cardPool[Random.Range(0, cardPool.Count)];
            thought.setCard(chosenCard);
            cardPool.Remove(chosenCard);
        }
    }
    

    public void clickCard(SumiWheelCardClick card)
    {
        MicrogameController.instance.playSFX(clickClip);

        //Find any match if we have one
        var thought = getMatchingThought(card);

        if (thought == null)
        {
            handleVictory(false);
        }
        else
        {
            thought.select();
            thoughtPool.Remove(thought);

            if (!thoughtPool.Any())
                handleVictory(true);
        }

    }

    void handleVictory(bool victory)
    {
        MicrogameController.instance.setVictory(victory, true);

        sumiRig.SetTrigger(victory ? "Victory" : "Failure");
        MicrogameController.instance.playSFX(victory ? victoryClip : lossClip);
    }

    SumiWheelThoughtCard getMatchingThought(SumiWheelCardClick card)
    {
        return thoughtPool.FirstOrDefault(a => a.getCard() == card);
    }
}
