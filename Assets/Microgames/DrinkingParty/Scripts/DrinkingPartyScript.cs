using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingPartyScript : MonoBehaviour
{
    public GameObject ObjectLiquide1;
    public GameObject ObjectLiquide2;
    public GameObject ObjectCara1;
    public GameObject ObjectCara2;
    public Sprite Cara1_Anim1;
    public Sprite Cara1_Anim2;
    public Sprite Cara1_Anim3;
    public Sprite Cara2_Anim1;
    public Sprite Cara2_Anim2;
    public Sprite Cara2_Anim3;
    public float percent_bottle1 = 100;
    public float percent_bottle2 = 100;
    
    private Vector3 Liquide1Scale;
    private Vector3 Liquide2Scale;
    private Vector3 Liquide1Pos;
    private Vector3 Liquide2Pos;

    // Use this for initialization
    void Start()
    {
        ObjectCara1.GetComponent<SpriteRenderer>().sprite = Cara1_Anim1;
        ObjectCara2.GetComponent<SpriteRenderer>().sprite = Cara2_Anim1;
    }

    void SetBottleBar()
    {
        Liquide1Scale.Set(0.1f, 0.45f * (percent_bottle1 / 100), 1f);
        Liquide2Scale.Set(0.1f, 0.45f * (percent_bottle2 / 100), 1f);
        Liquide1Pos.Set(-0.36f, 1.03f - (0.01345f * (100 - percent_bottle1)), 0f);//0.01345 = diff(up and down)/100
        Liquide2Pos.Set(0.67f, 1.03f - (0.01345f * (100 - percent_bottle2)), 0f);

        ObjectLiquide1.transform.localScale = Liquide1Scale;
        ObjectLiquide2.transform.localScale = Liquide2Scale;

        ObjectLiquide1.transform.position = Liquide1Pos; 
        ObjectLiquide2.transform.position = Liquide2Pos;

        UpdateAnimation();
        IsFinish();
    }

    void IsFinish ()
    {
        if(percent_bottle1 <= 0)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            //Do final animation ?
        }
    }

    void UpdateAnimation()
    {
        if(percent_bottle1<35)
        {
            ObjectCara1.GetComponent<SpriteRenderer>().sprite = Cara1_Anim3;
        }
        else if (percent_bottle1<75)
        {
            ObjectCara1.GetComponent<SpriteRenderer>().sprite = Cara1_Anim2;
        }

        if (percent_bottle2 < 35)
        {
            ObjectCara2.GetComponent<SpriteRenderer>().sprite = Cara2_Anim3;
        }
        else if (percent_bottle2 < 75)
        {
            ObjectCara2.GetComponent<SpriteRenderer>().sprite = Cara2_Anim2;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("space key was pressed");
            percent_bottle1 = percent_bottle1 - 5;
            percent_bottle2 = percent_bottle2 - 5;
            SetBottleBar();
        }
    }
}
