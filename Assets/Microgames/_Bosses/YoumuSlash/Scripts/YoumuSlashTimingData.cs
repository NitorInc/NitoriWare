using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/YoumuSlash/Timing Data")]
public class YoumuSlashTimingData : ScriptableObject
{

    [SerializeField]
    private AudioClip musicClip;
    public AudioClip MusicClip
    {
        get { return musicClip; }
    }

    [SerializeField]
    private float bpm = 130f;
    public float Bpm
    {
        get { return bpm; }
    }

    private AudioSource musicSource;

    public void initiate(AudioSource musicSource)
    {
        this.musicSource = musicSource;
    }

    public float getCurrentBeat()
    {
        if (!musicSource.isPlaying)
            return 0f;
        return musicSource.time / getBeatDuration();
    }

    public float getBeatDuration()
    {
        return 60f / bpm;
    }

}
