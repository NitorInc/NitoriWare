using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardHandCardGroup : MonoBehaviour {

    [SerializeField]
    public Sprite[] cards;

    [SerializeField]
    public Sprite cardBack;

    [SerializeField]
    public int cardCount = 4;
    [SerializeField]
    [Tooltip("Spacing width, in unity units")]
    public int cardWidth = 1;

    [SerializeField]
    public GameObject handObject;

    int selectedCard = 0;
    List<GameObject> cardObjects = new List<GameObject>();
    bool leftLast = false;
    bool rightLast = false;

    void MakeCard(int num, float x) {
        GameObject newObject = new GameObject($"Card{cardObjects.Count}");
        SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();

        renderer.sprite = (num == -1) ? this.cardBack : this.cards[num];

        CardHandCard newScript = newObject.AddComponent<CardHandCard>();
        newScript.value = num;
        newObject.transform.SetParent(this.transform);
        newObject.transform.position += new Vector3(this.transform.position.x + x,
            this.transform.position.y, this.transform.position.z);

        cardObjects.Add(newObject);
    }

	void Start () {
        float start_x = -cardWidth * cardCount / 2f;
        for (int i = 0; i < cardCount; i++) {
            MakeCard(i + 2, start_x + i*cardWidth + cardWidth / 2f);
        }

        PositionHand(false);
	}

    void PositionHand(bool glide) {
        if (selectedCard < 0) selectedCard = cardObjects.Count - 1;
        if (selectedCard >= cardObjects.Count) selectedCard = 0;

        Vector3 target = new Vector3(
            cardObjects[selectedCard].transform.position.x,
            cardObjects[selectedCard].transform.position.y - 0.75f,
            cardObjects[selectedCard].transform.position.y
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
    }
}
