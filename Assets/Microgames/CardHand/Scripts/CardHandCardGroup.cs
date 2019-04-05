using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardHandCardGroup : MonoBehaviour {
    [Header("The actual cards displayed")]

    [SerializeField]
    public int[] myCards;
    [SerializeField]
    public int[] theirCards;
    
    [SerializeField]
    public int mustSelect = 2;

    [Header("A load of sprites to assign to cards")]

    [SerializeField]
    public Sprite[] cards;
    [SerializeField]
    public Sprite cardBack;

    [SerializeField]
    [Tooltip("Spacing width, in unity units")]
    public int cardWidth = 1;

    [Header("Other important objects")]
    [SerializeField]
    public GameObject handObject;

    [SerializeField]
    public GameObject[] opponentCards;

    int selected = 0;
    int selectedCard = 0;
    List<GameObject> cardObjects = new List<GameObject>();
    bool leftLast = false;
    bool rightLast = false;
    bool spaceLast = false;

    void MakeCard(int num, float x) {
        GameObject newObject = new GameObject($"Card{cardObjects.Count}");
        SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();

        renderer.sprite = (num == -1) ? cardBack : cards[num];

        CardHandCard newScript = newObject.AddComponent<CardHandCard>();
        newScript.value = num;
        newObject.transform.SetParent(transform);
        newObject.transform.position += new Vector3(transform.position.x + x,
            transform.position.y, transform.position.z);

        cardObjects.Add(newObject);
    }

    void PrepareCards(int[] cardValues, int[] oppositionValues) {
        if (oppositionValues.Length != oppositionValues.Length) {
            Debug.LogWarning("Warning!! A different number of opposition cards has been passed as there are. " +
                             "Expect funky results and/or errors.");
        }

        float start_x = -cardWidth * (cardValues.Length) / 2f;
        for (int i = 0; i < cardValues.Length; i++) {
            MakeCard(cardValues[i], start_x + i * cardWidth + cardWidth / 2f);
        }

        for (int i = 0; i < oppositionValues.Length; i++) {
            opponentCards[i].GetComponent<CardHandOpponentCard>().value = oppositionValues[i];

            if (oppositionValues[i] == -1)
                opponentCards[i].GetComponent<SpriteRenderer>().sprite = cardBack;
            else
                opponentCards[i].GetComponent<SpriteRenderer>().sprite = cards[oppositionValues[i]];
        }

        PositionHand(false);
    }

    void PositionHand(bool glide) {
        if (selectedCard < 0) selectedCard = cardObjects.Count - 1;
        if (selectedCard >= cardObjects.Count) selectedCard = 0;

        bool chosen = cardObjects[selectedCard].GetComponent<CardHandCard>().IsSelected();
        Vector3 target = new Vector3(
            cardObjects[selectedCard].transform.position.x,
            transform.position.y - 0.75f,
            cardObjects[selectedCard].transform.position.z - 1
        );

        CardHandHand handScript = handObject.GetComponent<CardHandHand>();

        if (selectedCard == 0 && rightLast)
            handScript.dist = cardObjects.Count;
        else if (selectedCard == cardObjects.Count - 1 && leftLast)
            handScript.dist = cardObjects.Count;
        else handScript.dist = 1;

        handScript.target = target;
        if (!glide) handObject.transform.position = target;
    }

    void EvaluateWin() {
        int ourSum = 0;
        for (int i = 0; i < cardObjects.Count; i++) {
            CardHandCard cardScript = cardObjects[i].GetComponent<CardHandCard>();
            if (cardScript.value > -1)
                if (cardScript.IsSelected())
                    ourSum += cardScript.value;
        }

        int theirSum = 0;
        for (int i = 0; i < opponentCards.Length; i++) {
            CardHandOpponentCard cardScript = opponentCards[i].GetComponent<CardHandOpponentCard>();
            if (cardScript.value > -1)
                theirSum += cardScript.value;
        }

        if (ourSum > theirSum) print("Win");
        else print("Loss");

        MicrogameController.instance.setVictory(ourSum > theirSum);
    }

    void Start () {
        PrepareCards(myCards, theirCards);
	}

    void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow) && !leftLast) {
            leftLast = true;
            selectedCard--;
            PositionHand(true);
        } else leftLast = false;

        if (Input.GetKeyDown(KeyCode.RightArrow) && !rightLast) {
            rightLast = true;
            selectedCard++;
            PositionHand(true);
        } else rightLast = false;

        if (Input.GetKeyDown(KeyCode.Space) && !spaceLast) {
            if (selected < mustSelect && cardObjects[selectedCard].GetComponent<CardHandCard>().Select()) {
                selected++;
                
                if (selected == mustSelect)
                    EvaluateWin();
            }
            spaceLast = true;
        } else spaceLast = false;
    }
}
