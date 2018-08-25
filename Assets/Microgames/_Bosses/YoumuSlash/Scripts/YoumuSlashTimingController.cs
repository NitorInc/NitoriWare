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
    private float startDelay = .5f;
    [SerializeField]
    private int warmupBeats;
    [SerializeField]
    private AudioClip warmupBeatClip;

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
        lastInvokedBeat = -1 - warmupBeats;
        onBeat += checkForSongEnd;

        Invoke("beginWarmup", startDelay);
    }

    void beginWarmup()
    {
        float musicStartTime = (float)warmupBeats * timingData.BeatDuration;
        AudioHelper.playScheduled(musicSource, musicStartTime);
        callOnBeat();
        Invoke("callMusicStart", musicStartTime);
    }

    void callMusicStart()
    {
        if (!(onMusicStart == null))
            onMusicStart();
    }

    void callOnBeat()
    {
        lastInvokedBeat++;
        if (!(onBeat == null))
            onBeat(lastInvokedBeat);

        float nextBeatTime = (lastInvokedBeat + 1f) * timingData.BeatDuration;
        if (lastInvokedBeat >= 0)
            Invoke("callOnBeat", nextBeatTime - musicSource.time);
        else
            Invoke("callOnBeat", timingData.BeatDuration);

        if (lastInvokedBeat < 0)
            musicSource.PlayOneShot(warmupBeatClip);
    }

    private void Update()
    {
        if (MicrogameController.instance.isDebugMode())
        {
            float fastSpeed = 5f;
            if (Input.GetKeyDown(KeyCode.S))
            {
                Time.timeScale *= fastSpeed;
                musicSource.pitch *= fastSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                Time.timeScale /= fastSpeed;
                musicSource.pitch /= fastSpeed;
            }
        }
    }

    void checkForSongEnd(int beat)
    {
        if (lastInvokedBeat > 1 && !musicSource.isPlaying)
            CancelInvoke();
    }

}
