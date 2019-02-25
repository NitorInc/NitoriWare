using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceAutoPan : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float xPosBoundMult = 1f;
    
	void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        updatePan();
    }
	
	void LateUpdate()
    {
        updatePan();
	}

    void updatePan()
    {
        if (audioSource.isPlaying)
            audioSource.panStereo = AudioHelper.getAudioPan(transform.position.x, xPosBoundMult);
    }
}
