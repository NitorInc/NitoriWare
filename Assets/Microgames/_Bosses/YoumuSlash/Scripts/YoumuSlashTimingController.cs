using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YoumuSlashTimingController : MonoBehaviour
{
    public delegate void BeatDelegate(int beat);
    public delegate void SongStartDelegate();
    public delegate void FinalNoteDelegate();
    public static BeatDelegate onBeat;
    public static SongStartDelegate onMusicStart;
    public static FinalNoteDelegate onFinalNote;

    [SerializeField]
    private YoumuSlashTimingData timingData;
    [SerializeField]
    private YoumuSlashBeatMap beatMap;
    [SerializeField]
    private float startDelay = .5f;
    [SerializeField]
    private int[] warmupBeats;
    private Queue<int> warmupBeatQueue;
    [SerializeField]
    private AudioClip warmupBeatClip;

    private AudioSource musicSource;
    private int finalNote;

    private void Awake()
    {
        onBeat = null;
        onMusicStart = null;
        onFinalNote = null;

        warmupBeatQueue = new Queue<int>(warmupBeats);

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = timingData.MusicClip;
        timingData.initiate(musicSource, beatMap, warmupBeats.Count());
        finalNote = (int)beatMap.TargetBeats.Last().HitBeat;
    }

    void Start()
    {
        onBeat += checkForSongEnd;
        onBeat += onBeatLocal;

        Invoke("beginWarmup", startDelay);
    }

    void beginWarmup()
    {
        float musicStartTime = (float)warmupBeats.Sum() * timingData.BeatDuration;
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
        if (!(onBeat == null))
            onBeat(timingData.LastProcessedBeat + 1);
    }

    void onBeatLocal(int beat)
    {
        CancelInvoke("callOnBeat"); //This in case onBeat is force called from another script

        //Reinvoke onBeat
        if (beat >= 0)  //Normal beat
        {
            float nextBeatTime = (beat + 1f) * timingData.BeatDuration;
            Invoke("callOnBeat", nextBeatTime - musicSource.time);
        }
        else   //warmup beat
            Invoke("callOnBeat", timingData.BeatDuration * warmupBeatQueue.Dequeue());  //Dequeue warmup beat

        if (beat < 0)
            musicSource.PlayOneShot(warmupBeatClip);

        if (beat == finalNote)
            onFinalNote();
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
        if (timingData.LastProcessedBeat > 1 && !musicSource.isPlaying)
            CancelInvoke();
    }

}
