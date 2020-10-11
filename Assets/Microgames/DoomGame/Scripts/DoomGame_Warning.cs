using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Warning : MonoBehaviour {

    public DoomGame_Enemy[] enemies;
    public GameObject[] activates;
    public DoomGame_Player player;

    void Update () {
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i] != null)
                return;
        }
        MicrogameController.instance.displayLocalizedCommand("danger", "Behind you!");

        for (int i = 0; i < activates.Length; i++)
            activates[i].SetActive (true);
        player.setTurningAround();
        Destroy (this);
    }
}