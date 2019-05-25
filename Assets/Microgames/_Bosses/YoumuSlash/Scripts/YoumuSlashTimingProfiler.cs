using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to keep more precise track of input and reduce delay
public class YoumuSlashTimingProfiler : MonoBehaviour
{
    [SerializeField]
    private YoumuSlashTimingData timingData;

    private AudioSource musicSource;
    private float lastAudioTime = 0f;
    
	void Start ()
    {
        musicSource = GetComponent<AudioSource>();
    }

    void Update() => profile();
    void LateUpdate() => profile();
    void FixedUpdate() => profile();
    void OnGUI() => profile();

    void profile()
    {
        if (lastAudioTime != musicSource.time)
        {
            lastAudioTime = musicSource.time;
            timingData.SetLastUpdateTime();
        }
    }
}
