using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTimingController : MonoBehaviour
{
    public delegate void BeatDelegate(int beat);
    public static BeatDelegate onBeat;

    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private float StartDelay = .5f;

    private AudioSource musicSource;

    private int lastInvokedBeat;

    private void Awake()
    {
        onBeat = null;
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = timingData.MusicClip;
        timingData.initiate(musicSource);
    }

    void Start()
    {
        AudioHelper.playScheduled(musicSource, StartDelay);
        lastInvokedBeat = -1;
        onBeat += checkForSongEnd;
        InvokeRepeating("callOnBeat", StartDelay, timingData.getBeatDuration());
    }

    void callOnBeat()
    {
        lastInvokedBeat++;
        onBeat(lastInvokedBeat);
    }

    void checkForSongEnd(int beat)
    {
        print("Debug beat check");
        if (lastInvokedBeat > 1 && !musicSource.isPlaying)
            CancelInvoke();
    }

}
