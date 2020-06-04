using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameLetterMovement : MonoBehaviour
{
    [SerializeField]
    private AudioClip grabClip;
    [SerializeField]
    private AudioClip releaseClip;
    [SerializeField]
    private float grabPitch;
    [SerializeField]
    private float releasePitch;
    [SerializeField]
    private float grabScaleMult = 1.05f;

    private bool grabbed;
    private Vector3 initialScale;
    
	void Start ()
    {
        initialScale = transform.localScale;
	}
	
	void Update ()
    {
		
	}


    public void setGrab(bool grabbed)
    {
        this.grabbed = grabbed;
        transform.localScale = initialScale * (grabbed ? grabScaleMult : 1f);
        MicrogameController.instance.playSFX(grabbed ? grabClip : releaseClip,
            panStereo: AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x),
            pitchMult: grabbed ? grabPitch : releasePitch);
    }
}
