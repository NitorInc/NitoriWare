using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoomGame_UI : MonoBehaviour
{
    [SerializeField]
    RectTransform rArrow, lArrow;
    [SerializeField]
    Image reisen;
    [SerializeField]
    Sprite[] reisenSprites;

    public static bool rightArrow, leftArrow;

    private float counter = 0;
    private int id = 0;

    void Start()
    {

    }

    void Update()
    {
        counter += Time.deltaTime;
        if(counter > 0.5f)
        {
            int last = id;
            while(last == id)
                id = Random.Range(0, 3);
            counter = 0;
        }
        if(MicrogameController.instance.getVictoryDetermined())
        {
            if(MicrogameController.instance.getVictory())
                reisen.sprite = reisenSprites[5];
            else
                reisen.sprite = reisenSprites[4];
        }
        else
            reisen.sprite = reisenSprites[id];

        if(rArrow.gameObject.activeSelf != rightArrow)
            rArrow.gameObject.SetActive(rightArrow);
        if(lArrow.gameObject.activeSelf != leftArrow)
            lArrow.gameObject.SetActive(leftArrow);
    }
}
