using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingSimPlaceholderMusic : MonoBehaviour
{
    public float startDelayInBeats = 0f;
    public List<AudioClip> characterClips;

    private AudioSource audioSource;
    
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        DialoguePreset.OnCharacterSelection += SelectTrack;
	}

    void SelectTrack(int index)
    {
        audioSource.clip = characterClips[index];
        audioSource.pitch = Time.timeScale;
        Invoke("PlayTrack", startDelayInBeats * (60f / 130f));
    }

    void PlayTrack()
    {
        audioSource.Play();
    }
}
