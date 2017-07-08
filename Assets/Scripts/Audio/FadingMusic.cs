using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingMusic : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private float fadeSpeed;
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
        float diff = fadeSpeed * Time.deltaTime;
        if (_audioSource.volume <= diff)
        {
            _audioSource.volume = 0f;
            _audioSource.Stop();
        }
        else
            _audioSource.volume -= diff;
	}

    public void startFade()
    {
        started = true;
    }
}
