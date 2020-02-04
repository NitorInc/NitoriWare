﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameReceiverGrabLetter : MonoBehaviour
{
    [SerializeField]
    GayGameSenderGrabLetter senderGrabLetter;
    [SerializeField]
    private Transform letterTransform;
    [SerializeField]
    private Transform newLetterParent;
    [SerializeField]
    private Animator sceneAnimator;
    [SerializeField]
    private GameObject grabbablesObject;
    [SerializeField]
    private GameObject[] enableObjects;
    [SerializeField]
    private GameObject[] disableObjects;
    [SerializeField]
    private AudioClip[] grabClips;
    [SerializeField]
    private bool requireMouseUp;

    private bool inCollider = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        updateCollision(collision, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        updateCollision(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        updateCollision(collision, false);
    }

    void updateCollision(Collider2D collision, bool inCollider)
    {
        if (senderGrabLetter.Grabbed && collision.tag.Equals("MicrogameTag2"))
        {
            this.inCollider = inCollider;
        }
    }

    private void Update()
    {
        if (inCollider && (Input.GetMouseButtonUp(0) || !requireMouseUp))
        {
            grab();
            enabled = false;
        }
    }

    void grab()
    {
        MicrogameController.instance.setVictory(true);
        letterTransform.parent = newLetterParent;
        sceneAnimator.SetTrigger("Victory");
        sceneAnimator.SetFloat("BegSpeed", 0f);
        foreach (var grabbable in grabbablesObject.GetComponents<MouseGrabbable>())
        {
            grabbable.enabled = false;
        }
        foreach (var enObj in enableObjects)
        {
            enObj.SetActive(true);
        }
        foreach (var disObj in disableObjects)
        {
            disObj.SetActive(false);
        }
        foreach (var grabClip in grabClips)
        {
            MicrogameController.instance.playSFX(grabClip, panStereo: AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x));
        }
    }
}
