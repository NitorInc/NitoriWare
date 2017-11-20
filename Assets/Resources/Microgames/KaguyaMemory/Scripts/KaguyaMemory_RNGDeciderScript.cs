using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_RNGDeciderScript : MonoBehaviour
{

    public GameObject[] items;
    public int maxItems = 3;
    public float showDelay = 1;
    public float speedMultiplier = 1;
    public bool finished = false;

    [SerializeField]
    private AudioClip whooshSound;

    private GameObject chosenItem;
    private int timer1 = 0;

    void Start()
    {
        //select an item
        chosenItem = items[Random.Range(0, maxItems)];
        chosenItem.GetComponent<KaguyaMemory_ItemScript>().isCorrect = true;

        Invoke("ShowItem", showDelay);
    }

    void ShowItem()
    {
        chosenItem.transform.position = new Vector3(-4, -1, 0);
        chosenItem.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        chosenItem.GetComponent<Rigidbody2D>().velocity = new Vector2(10 * speedMultiplier, 8);
        chosenItem.GetComponent<Rigidbody2D>().gravityScale = 2 * speedMultiplier;

        MicrogameController.instance.playSFX(whooshSound, volume: 0.5f,
        panStero: AudioHelper.getAudioPan(transform.position.x));
    }
}
