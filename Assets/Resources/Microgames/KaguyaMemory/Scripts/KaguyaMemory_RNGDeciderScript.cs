using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_RNGDeciderScript : MonoBehaviour
{

    public GameObject[] items;
    public GameObject KaguyaChan;
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
        KaguyaChan.transform.position += new Vector3(15, 0, 0);

        //randomize item positions
        int randomLoops = Random.Range(50, 100);
        for (int i = 0; i < randomLoops; i++)
        {
            for (int j = 0; j < maxItems - 1; j++)
            {
                int randomChance = Random.Range(0, 100);
                if (randomChance < 50)
                {
                    int randomItem = Random.Range(0, maxItems);
                    Vector3 tempPosition = items[j].transform.position;
                    items[j].transform.position = items[randomItem].transform.position;
                    items[randomItem].transform.position = tempPosition;
                }
            }
        }

        Invoke("ShowItem", showDelay);
    }

    void ShowItem()
    {
        chosenItem.transform.position = new Vector3(-4, -1, 0);
        chosenItem.GetComponent<SpriteRenderer>().enabled = true;
        KaguyaChan.transform.position -= new Vector3(15, 0, 0);
        chosenItem.GetComponent<Rigidbody2D>().velocity = new Vector2(10 * speedMultiplier, 8);
        chosenItem.GetComponent<Rigidbody2D>().gravityScale = 2 * speedMultiplier;

        MicrogameController.instance.playSFX(whooshSound, volume: 0.5f,
        panStereo: AudioHelper.getAudioPan(transform.position.x));
    }
}
