using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/YoumuSlash/Timing Data")]
public class YoumuSlashTimingData : ScriptableObject
{

    [SerializeField]
    private AudioClip musicClip;
    public AudioClip MusicClip => musicClip;

    [SerializeField]
    private float bpm = 130f;
    public float Bpm => bpm;

    public YoumuSlashBeatMap BeatMap { get; private set; }

    private AudioSource musicSource;

    public float CurrentBeat
    {
        get
        {
            if (!musicSource.isPlaying)
                return 0f;
            return musicSource.time / BeatDuration;
        }
    }

    public float BeatDuration => 60f / bpm;

    public void initiate(AudioSource musicSource, YoumuSlashBeatMap beatMap)
    {
        this.musicSource = musicSource;
        this.BeatMap = beatMap;
        beatMap.initiate();
    }

}
