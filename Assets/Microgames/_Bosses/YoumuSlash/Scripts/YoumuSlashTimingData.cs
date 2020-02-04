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
    
    private YoumuSlashBeatMap beatMap;
    public YoumuSlashBeatMap BeatMap => beatMap;

    private AudioSource musicSource;
    private int lastProcessedBeat;
    public int LastProcessedBeat => lastProcessedBeat;

    float lastBeatRealTime;

    public float CurrentBeat
    {
        get
        {
            if (!musicSource.isPlaying)
                return lastReportedBeat;
            var beat = musicSource.time / BeatDuration;
            lastReportedBeat = beat;
            return beat;
        }
    }
    
    private float beatMax;
    public float PreciseBeat
    {
        get
        {
            if (!musicSource.isPlaying || musicSource.time <= 0f)
                return lastReportedBeat;
            var timeSinceAudioUpdate = Time.realtimeSinceStartup - lastUpdateTime;
            if (timeSinceAudioUpdate > .4f || timeSinceAudioUpdate < 0f)
            {
                timeSinceAudioUpdate = 0f;
                SetLastUpdateTime();
            }
            var realMusicTime = musicSource.time + (timeSinceAudioUpdate / musicSource.pitch);

            var beat = realMusicTime / BeatDuration;
            lastReportedBeat = beat;
            return beat;
        }
    }

    private float lastUpdateTime;
    public void SetLastUpdateTime() => lastUpdateTime = Time.realtimeSinceStartup;

    float lastReportedBeat;

    public float BeatDuration => 60f / bpm;

    public void initiate(AudioSource musicSource, YoumuSlashBeatMap beatMap, int warmupBeatCount)
    {
        this.musicSource = musicSource;
        this.beatMap = beatMap;
        lastProcessedBeat = -1 - warmupBeatCount;
        beatMap.initiate();
        YoumuSlashTimingController.onBeat += updateBeatCount;
        SetLastUpdateTime();
        lastReportedBeat = 0f;
    }

    public void updateBeatCount(int beat)
    {
        lastProcessedBeat = beat;
    }

}
