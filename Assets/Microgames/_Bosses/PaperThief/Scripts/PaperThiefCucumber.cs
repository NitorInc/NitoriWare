using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefCucumber : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 holdOffset;
    [SerializeField]
    private SineWave sineWave;
    [SerializeField]
    private AudioClip grabClip;
    [SerializeField]
    private float grabClipDelay;
#pragma warning restore 0649

    private Vector3 goalPosition;

    void Update()
    {
        if (goalPosition != Vector3.zero)
        {
            goalPosition = PaperThiefNitori.instance.transform.position + holdOffset;
            if (transform.moveTowards(goalPosition, moveSpeed))
            {
                Invoke("playGrabClip", grabClipDelay);
                goalPosition = Vector3.zero;
            }
        }
    }

    void playGrabClip()
    {
        MicrogameController.instance.playSFX(grabClip);
    }

    public void collect()
    {
        sineWave.enabled = false;
        goalPosition = PaperThiefNitori.instance.transform.position + holdOffset;

    }
}
