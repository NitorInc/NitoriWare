using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashTimingEffectsController : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private float pitchMult = 1f;
    [SerializeField]
    private float timeScaleMult = 1f;
    [SerializeField]
    private float volumeMult = 1f;
    
    bool failed = false;
    bool ended = false;
    float initialTimeScale;
    float initialVolume;
    
	void Start ()
    {
        YoumuSlashPlayerController.onFail += onFail;
        YoumuSlashPlayerController.onGameplayEnd += onGameplayEnd;
        initialTimeScale = Time.timeScale;
        initialVolume = musicSource.volume;
	}

    void onGameplayEnd()
    {
        ended = true;
    }
	
	void onFail()
    {
        failed = true;
	}

    private void LateUpdate()
    {
        if (MicrogameController.instance.getVictoryDetermined())
            Time.timeScale = initialTimeScale;
        else if (ended)
            Time.timeScale = timeScaleMult * initialTimeScale;

        if (failed)
            musicSource.pitch = Time.timeScale * pitchMult;

        musicSource.volume = volumeMult * initialVolume;
    }
}
