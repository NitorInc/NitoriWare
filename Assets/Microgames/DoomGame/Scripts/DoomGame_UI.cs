using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoomGame_UI : MonoBehaviour
{
    [SerializeField]
    Image reisen, heart;
    [SerializeField]
    Image[] ammo;
    [SerializeField]
    Sprite emptyHeart;
    [SerializeField]
    Sprite[] reisenSprites, ammoSprites;

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
    }

    public void UpdateAmmo(int value)
    {
        value--;
        for(int i = 0; i < ammo.Length; i++)
        {
            if(value > i * 2)
                ammo[i].sprite = ammoSprites[0];
            else if(value > i * 2 - 1)
                ammo[i].sprite = ammoSprites[1];
            else
                ammo[i].sprite = ammoSprites[2];
        }
    }

    public void Die()
    {
        heart.sprite = emptyHeart;
    }
}
