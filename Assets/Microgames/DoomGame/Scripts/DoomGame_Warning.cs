using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomGame_Warning : MonoBehaviour {

    public DoomGame_Enemy[] enemies;
    public GameObject[] activates;

    void Update () {
        for (int i = 0; i < enemies.Length; i++) {
            if (enemies[i] != null)
                return;
        }
        MicrogameController.instance.displayLocalizedCommand("danger", "Behind you!",
            MicrogameController.instance.getTraits().commandAnimatorOverride);

        for (int i = 0; i < activates.Length; i++)
            activates[i].SetActive (true);
        Destroy (this);
    }
}