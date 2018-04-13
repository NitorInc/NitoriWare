using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTimingController : MonoBehaviour
{
    public delegate void BeatDelegate(int beat);
    public delegate void SongStartDelegate();
    public static BeatDelegate onBeat;
    public static SongStartDelegate onMusicStart;

    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private YoumuSlashBeatMap beatMap;
    [SerializeField]
    private float StartDelay = .5f;

    private AudioSource musicSource;

    private int lastInvokedBeat;

    private void Awake()
    {
        onBeat = null;
        onMusicStart = null;

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = timingData.MusicClip;
        timingData.initiate(musicSource, beatMap);
    }

    void Start()
    {
        lastInvokedBeat = -1;
        onBeat += checkForSongEnd;
        AudioHelper.playScheduled(musicSource, StartDelay);
        Invoke("callMusicStart", StartDelay);
    }

    void callMusicStart()
    {
        onMusicStart();
        callOnBeat();
    }

    void callOnBeat()
    {
        lastInvokedBeat++;
        onBeat(lastInvokedBeat);

        float nextBeatTime = (lastInvokedBeat + 1f) * timingData.BeatDuration;
        Invoke("callOnBeat", nextBeatTime - musicSource.time);
    }

    void checkForSongEnd(int beat)
    {
        print("Beat " + beat);
        if (lastInvokedBeat > 1 && !musicSource.isPlaying)
            CancelInvoke();
    }

}
