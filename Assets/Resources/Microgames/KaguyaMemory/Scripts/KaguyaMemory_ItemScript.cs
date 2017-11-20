using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_ItemScript : MonoBehaviour {

    public GameObject rngMaster;
    public GameObject correctIndicator;
    public GameObject wrongIndicator;
    public bool isMoving = false;
    public bool isCorrect = false;

    private Vector3 startingPosition;
    private bool isSelectable = false;

    [SerializeField]
    private AudioClip correctSound;

    [SerializeField]
    private AudioClip wrongSound;

    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        startingPosition = transform.position;

        Invoke("appearSelectable", 2.3f);
    }

    void OnMouseDown()
    {
        if(rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished == false && isSelectable == true)
        {
            GameObject theIndicator;
            if (isCorrect == true)
            {
                theIndicator = Instantiate(correctIndicator);
                MicrogameController.instance.setVictory(true, true);
                MicrogameController.instance.playSFX(correctSound, volume: 0.5f,
                panStero: AudioHelper.getAudioPan(0));
            }
            else
            {
                theIndicator = Instantiate(wrongIndicator);
                MicrogameController.instance.setVictory(false, true);
                MicrogameController.instance.playSFX(wrongSound, volume: 0.5f,
                panStero: AudioHelper.getAudioPan(0));
            }
            theIndicator.transform.position = transform.position;
            rngMaster.GetComponent<KaguyaMemory_RNGDeciderScript>().finished = true;
            isSelectable = false;
        }
        
    }

    void appearSelectable()
    {
        transform.position = startingPosition;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().gravityScale = 0;
        isSelectable = true;
    }
}
