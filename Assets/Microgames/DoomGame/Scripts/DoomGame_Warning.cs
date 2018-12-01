using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Warning : MonoBehaviour
{

    public DoomGame_Enemy[] enemies;

    void Update()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
                return;
        }
        MicrogameController.instance.displayLocalizedCommand("danger", "Behind you!");
        Destroy(this);
    }
}
