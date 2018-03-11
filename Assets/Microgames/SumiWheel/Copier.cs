using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Copier : MonoBehaviour {

    public bool copy;
    public bool paste;

    public Card[] cards;
    [System.Serializable]
    public struct Card
    {
        public Vector3 position;
        public Quaternion rotation;
        public Sprite cardSprite;
        public Sprite iconSprite;
        public Color iconColor;
    }


	// Use this for initialization
	public void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (copy)
            cards = new Card[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            if (copy)
            {
                var child = transform.GetChild(i);
                cards[i].position = child.position;
                cards[i].rotation = child.rotation;
                cards[i].cardSprite = child.GetComponent<SpriteRenderer>().sprite;
                cards[i].iconColor = child.GetChild(0).GetComponent<SpriteRenderer>().color;
                cards[i].iconSprite = child.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            }

            if (paste)
            {
                var child = transform.GetChild(i);
                child.position = cards[i].position;
                child.rotation = cards[i].rotation;
                child.GetComponent<SpriteRenderer>().sprite = cards[i].cardSprite;
                child.GetChild(0).GetComponent<SpriteRenderer>().color = cards[i].iconColor;
                child.GetChild(0).GetComponent<SpriteRenderer>().sprite = cards[i].iconSprite;
            }


        }

        copy = paste = false;
    }
}
