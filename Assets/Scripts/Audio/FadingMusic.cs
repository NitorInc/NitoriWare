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
    private PrefsHelper.VolumeType type = PrefsHelper.VolumeType.Music;
#pragma warning restore 0649

    private AudioSource _audioSource;
    private bool started;

	void Awake()
	{
        started = false;
        _audioSource = GetComponent<AudioSource>();
	}

    void Update()
    {
        if (started)
            updateFade();
    }

	
	void updateFade()
	{
        if (PrefsHelper.getVolume(type) <= 0f)
            return;

        float volumeMult = PrefsHelper.getVolume(type);
        float diff = fadeSpeed * Time.deltaTime * volumeMult;
        if (fadeInFirst)
        {
            if (_audioSource.volume >= volumeMult)
            {
                _audioSource.volume = volumeMult;
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
