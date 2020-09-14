using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanBlowFanSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    private float playDistance = 5f;
    [SerializeField]
    private float ZeroVolumeSpeed = 10f;
    [SerializeField]
    private float OneVolumeSpeed = 50f;
    [SerializeField]
    private Vector2 pitchRange = new Vector2(.95f, 1.05f);

    private Vector3 lastPosition;
    private AudioSource sfxSource;
    private float progress;

	void Start ()
    {
        lastPosition = transform.localPosition;
        sfxSource = GetComponent<AudioSource>();
	}
	
	void Update ()
    {
        var distance = (transform.localPosition - lastPosition).magnitude;
        progress += distance;
        if (progress > playDistance)
        {
            progress -= playDistance;
            var speed = distance / Time.deltaTime;
            var volume = Mathf.InverseLerp(ZeroVolumeSpeed, OneVolumeSpeed, speed);
            if (volume > 0f)
                sfxSource.PlayOneShot(audioClip, volume);
            sfxSource.pitch = MathHelper.randomRangeFromVector(pitchRange);
        }
        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x, .7f);
        lastPosition = transform.localPosition;
    }
}
