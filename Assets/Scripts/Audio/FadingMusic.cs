using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingMusic : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private bool fadeInFirst;
    [SerializeField]
    private float fadeSpeed;
    [SerializeField]
    private bool startOnAwake;
    [SerializeField]
    private PrefsHelper.VolumeType type = PrefsHelper.VolumeType.Music;
#pragma warning restore 0649

    private AudioSource _audioSource;
    private bool started;
    private float goalVolumeMult = 1f;

	void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        started = startOnAwake;
        if (fadeInFirst && startOnAwake)
        {
            goalVolumeMult = _audioSource.volume;
            _audioSource.volume = 0f;
        }
	}

    void Update()
    {
        if (started)
            updateFade();
    }

	
	void updateFade()
	{
        float prefsMult = 1f;
        float diff = fadeSpeed * Time.deltaTime * prefsMult;
        if (fadeInFirst)
        {
            if (_audioSource.volume >= prefsMult * goalVolumeMult)
            {
                _audioSource.volume = prefsMult * goalVolumeMult;
                fadeInFirst = started = false;
            }
            else
                _audioSource.volume += diff;
        }
        else
        {
            if (_audioSource.volume <= diff)
            {
                _audioSource.volume = 0f;
                fadeInFirst = true;
                started = false;
            }
            else
                _audioSource.volume -= diff;
        }
	}

    public void startFade()
    {
        if (started)
            fadeInFirst = !fadeInFirst;
        started = true;
    }
}
