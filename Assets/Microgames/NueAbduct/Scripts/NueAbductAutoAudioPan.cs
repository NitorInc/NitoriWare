using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NueAbductAutoAudioPan : MonoBehaviour {

    // Add this component to automatically pan the attached AudioSource
    // to the transform's position

    [SerializeField]
    private AudioSource audioSource;

    // Use this for initialization
    void Start() {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        audioSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
    }
}
