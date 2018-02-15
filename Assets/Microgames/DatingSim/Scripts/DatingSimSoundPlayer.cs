using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimSoundPlayer : MonoBehaviour
{
    public AudioClip winClip, lossClip;

    void onResult(bool victory)
    {
        MicrogameController.instance.playSFX(victory ? winClip : lossClip);
    }
}
