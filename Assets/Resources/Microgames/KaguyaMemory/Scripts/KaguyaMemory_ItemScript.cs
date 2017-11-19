using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemory_ItemScript : MonoBehaviour {

    [SerializeField]
    private AudioClip correctSound;

    [SerializeField]
    private AudioClip wrongSound;

    public GameObject rngMaster;
    private Vector3 startingPosition;
    private int timer1 = 1;
    public bool isMoving = false;
    public bool isCorrect = false;
    public GameObject correctIndicator;
    public GameObject wrongIndicator;
    public bool isClicked = false;

	// Use this for initialization
	void Start () {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        startingPosition = transform.position;
	}

    void OnMouseDown()
    {
        if(isClicked == false)
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
            isClicked = true;
        }
        
    }

    // Update is called once per frame
    void Update () {
        if (timer1 > 0)
        {
            timer1++;
        }
        if (timer1 == 130)
        {
            transform.position = startingPosition;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().gravityScale = 0;
        }
	}
}
