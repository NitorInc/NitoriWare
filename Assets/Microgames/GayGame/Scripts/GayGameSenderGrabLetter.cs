using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameSenderGrabLetter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enableOnGrab;
    [SerializeField]
    private GameObject[] disableOnGrab;
    [SerializeField]
    private MouseGrabbable letterHitBoxGrabbable;
    [SerializeField]
    private AudioClip grabClip;
    [SerializeField]
    private bool grabAtStart;

    private bool grabbed = false;
    public bool Grabbed => grabbed;

    void Start ()
    {
        if (grabAtStart)
            grab();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("MicrogameTag1"))
        {
            grab();
        }
    }

    void grab()
    {
        grabbed = true;
        foreach (var obj in enableOnGrab)
        {
            obj.SetActive(true);
        }
        foreach (var obj in disableOnGrab)
        {
            obj.SetActive(false);
        }
        letterHitBoxGrabbable.enabled = true;
        if (!grabAtStart)
            MicrogameController.instance.playSFX(grabClip, panStereo: AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x));
    }
}
