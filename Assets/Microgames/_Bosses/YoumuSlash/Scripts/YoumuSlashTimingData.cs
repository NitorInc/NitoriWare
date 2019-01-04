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

    [SerializeField]
    private YoumuSlashBeatMap beatMap;
    public YoumuSlashBeatMap BeatMap => beatMap;

    private AudioSource musicSource;
    private int lastProcessedBeat;
    public int LastProcessedBeat => lastProcessedBeat;

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

    public void initiate(AudioSource musicSource, YoumuSlashBeatMap beatMap, int warmupBeatCount)
    {
        this.musicSource = musicSource;
        this.beatMap = beatMap;
        lastProcessedBeat = -1 - warmupBeatCount;
        beatMap.initiate();
        YoumuSlashTimingController.onBeat += updateBeatCount;
    }

    public void updateBeatCount(int beat)
    {
        lastProcessedBeat = beat;
    }

}
