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
    [SerializeField]
    private AudioClip debugBeatClip;

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

        musicSource.PlayOneShot(debugBeatClip);
    }

    private void Update()
    {
        if (MicrogameController.instance.isDebugMode())
        {
            float fastSpeed = 2.5f;
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
