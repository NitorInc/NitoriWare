using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckGhostController : MonoBehaviour {
    [SerializeField]
    private float ghostcount;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    void killaghost()
    {
        ghostcount = ghostcount - 1f;
        if (ghostcount == 0f)
        {
            MicrogameController.instance.setVictory(true, true);
        }
}
}
