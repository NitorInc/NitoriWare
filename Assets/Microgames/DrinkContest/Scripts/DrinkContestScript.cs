using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkContestScript : MonoBehaviour
{
    public GameObject ObjectLiquide1;
    public GameObject ObjectLiquide2;
    public GameObject ObjectCara1;
    public GameObject ObjectCara2;
    public Sprite Cara1Anim1;
    public Sprite Cara1Anim2;
    public Sprite Cara1Anim3;
    public Sprite Cara2Anim1;
    public Sprite Cara2Anim2;
    public Sprite Cara2Anim3;
    public float percentbottle1 = 100;
    public float percentbottle2 = 100;

    public float PercentLooseClick = 10;
    private Vector3 Liquide1Scale;
    private Vector3 Liquide2Scale;
    private Vector3 Liquide1Pos;
    private Vector3 Liquide2Pos;

    [SerializeField]
    private AudioClip DrinkSound;

    private float Time0;
    private float TotalTime;
    private int i = 0;
    private float j = 100;

    // Use this for initialization
    void Start()
    {
        //Init Anim to First frame
        ObjectCara1.GetComponent<SpriteRenderer>().sprite = Cara1Anim1;
        ObjectCara2.GetComponent<SpriteRenderer>().sprite = Cara2Anim1;
        TotalTime = 8 * StageController.beatLength;
        i = 0;
        Time0 = Time.time;
    }

    void SetBottleBar()
    {
        //Rescale and Position fct
        Liquide1Scale.Set(0.1f, 0.45f * (percentbottle1 / 100), 1f);
        Liquide2Scale.Set(0.1f, 0.45f * (percentbottle2 / 100), 1f);
        //0.01345 = diff(up and down)/100
        Liquide1Pos.Set(-0.395f, 1.0f - (0.01345f * (100 - percentbottle1)), 0f);
        Liquide2Pos.Set(0.647f, 1.0f - (0.01345f * (100 - percentbottle2)), 0f);

        //Change the scale (reduce Y)
        ObjectLiquide1.transform.localScale = Liquide1Scale;
        ObjectLiquide2.transform.localScale = Liquide2Scale;

        //Change Position (go buttom)
        ObjectLiquide1.transform.position = Liquide1Pos;
        ObjectLiquide2.transform.position = Liquide2Pos;

        UpdateAnimation(); //Update the animation if is needed
        IsFinish(); //Test if you won
    }

    void IsFinish()
    {
        if (percentbottle1 <= 0)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            //Do final animation ?
        }
    }

    void UpdateAnimation()
    {
        if ((percentbottle1 < 35)&& percentbottle1 > 0 && (ObjectCara1.GetComponent<SpriteRenderer>().sprite != Cara1Anim3))
        {
            ObjectCara1.GetComponent<SpriteRenderer>().sprite = Cara1Anim3;
        }
        else if (percentbottle1 < 75 && percentbottle1 >= 35 && (ObjectCara1.GetComponent<SpriteRenderer>().sprite != Cara1Anim2))
        {
            ObjectCara1.GetComponent<SpriteRenderer>().sprite = Cara1Anim2;
        }

        if (percentbottle2 < 35 && percentbottle2 > 0 && (ObjectCara2.GetComponent<SpriteRenderer>().sprite != Cara2Anim3))
        {
            ObjectCara2.GetComponent<SpriteRenderer>().sprite = Cara2Anim3;
        }
        else if (percentbottle2 < 75 && percentbottle2 >= 35 && (ObjectCara2.GetComponent<SpriteRenderer>().sprite != Cara2Anim2))
        {
            ObjectCara2.GetComponent<SpriteRenderer>().sprite = Cara2Anim2;
        }
    }

    void IAGameplay()
    {
        if (((Time.time - Time0) / TotalTime) > ((i * PercentLooseClick) / 100))
        {
            percentbottle2 = percentbottle2 - PercentLooseClick;
            SetBottleBar();
            i++;
        }
    }

    void PlaySound()
    {
        MicrogameController.instance.playSFX(DrinkSound, volume: 0.5f,
            panStereo: AudioHelper.getAudioPan(transform.position.x));
    }

    void ConditionPlayingSound()
    {
        if (percentbottle1 < j)
        {
            print("Play sound");
            PlaySound();
            j -= 10;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("space key was pressed");
            percentbottle1 = percentbottle1 - PercentLooseClick;
            SetBottleBar();
            ConditionPlayingSound();
        }
        IAGameplay();
    }


}

