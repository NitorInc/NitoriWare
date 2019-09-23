using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceAutoPan : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float xPosScaleMult = 1f;
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
        var mainCam = MainCameraSingleton.instance;
        if (audioSource.isPlaying && mainCam != null)
        {
            var pan = AudioHelper.getAudioPan(transform.position.x, mainCam, xPosBoundMult) * xPosScaleMult;
            pan = Mathf.Clamp(pan, -1f, 1f);
            audioSource.panStereo = pan;
        }
    }
}
