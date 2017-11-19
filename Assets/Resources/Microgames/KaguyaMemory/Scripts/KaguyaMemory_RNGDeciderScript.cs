using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_RNGDeciderScript : MonoBehaviour {

    [SerializeField]
    private AudioClip whooshSound;

    public bool finished = false;
    public int maxItems = 3;
    private int randomNumber = 0;
    public GameObject item1;
    public GameObject item2;
    public GameObject item3;
    public GameObject item4;
    public GameObject item5;
    private GameObject chosenItem;
    private int timer1 = 0;
    public float speedMultiplier = 1;

	// Use this for initialization
	void Start () {
        //select an item
        chosenItem = item1.GetComponent<GameObject>();
        randomNumber = Random.Range(1,maxItems + 1);
        if (randomNumber == 1)
        {
            chosenItem = item1;
        }
        if (randomNumber == 2)
        {
            chosenItem = item2;
        }
        if (randomNumber == 3)
        {
            chosenItem = item3;
        }
        if (randomNumber == 4)
        {
            chosenItem = item4;
        }
        if (randomNumber == 5)
        {
            chosenItem = item5;
        }

        chosenItem.GetComponent<KaguyaMemory_ItemScript>().isCorrect = true;
    }

    // Update is called once per frame
    void Update() {
        if (finished == true)
        {
            item1.GetComponent<KaguyaMemory_ItemScript>().isClicked = true;
            item2.GetComponent<KaguyaMemory_ItemScript>().isClicked = true;
            item3.GetComponent<KaguyaMemory_ItemScript>().isClicked = true;
            item4.GetComponent<KaguyaMemory_ItemScript>().isClicked = true;
            item5.GetComponent<KaguyaMemory_ItemScript>().isClicked = true;
        }

        if (timer1 == 40)
        {
            chosenItem.transform.position = new Vector3(-4, -1, 0);
            chosenItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            chosenItem.GetComponent<Rigidbody2D>().velocity = new Vector2(10 * speedMultiplier, 8);
            chosenItem.GetComponent<Rigidbody2D>().gravityScale = 2 * speedMultiplier;
            
            MicrogameController.instance.playSFX(whooshSound, volume: 0.5f,
            panStero: AudioHelper.getAudioPan(transform.position.x));
        }
        timer1++;
	}
}
