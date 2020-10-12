using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayGameSenderMovement : MonoBehaviour
{
    [SerializeField]
    private float scalePerY = .25f;
    [SerializeField]
    private float maxY = 3f;
    [SerializeField]
    private float anglePerX = 5f;
    [SerializeField]
    private AudioClip grabClip;
    [SerializeField]
    private AudioClip releaseClip;
    [SerializeField]
    private float grabPitch;
    [SerializeField]
    private float releasePitch;
    [SerializeField]
    private float grabVolume = .8f;
    [SerializeField]
    private float releaseVolume = .8f;

    private Vector3 startPosition;
    private Vector3 initialScale;
    bool grabbed;
    public bool Grabbed => grabbed;

    void Start ()
    {
        startPosition = transform.position;
        initialScale = transform.localScale;
	}
	
	public void LateUpdate ()
    {
        if (transform.position.y > maxY)
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        float currentScale = 1f + (transform.position.y - startPosition.y) * scalePerY;
        transform.localScale = initialScale * currentScale;
        float currentAngle = (startPosition.x - transform.position.x) * anglePerX;
        transform.localEulerAngles = Vector3.forward * currentAngle;
	}

    public void setGrab(bool grabbed)
    {
        this.grabbed = grabbed;
        MicrogameController.instance.playSFX(grabbed ? grabClip : releaseClip,
            panStereo: AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x),
            pitchMult: grabbed ? grabPitch : releasePitch);
    }
}
