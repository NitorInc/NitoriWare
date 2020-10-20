using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMicrogameMusic : MonoBehaviour
{

    void Start()
    {
        var musicSource = GetComponent<AudioSource>();
        if (musicSource.clip != null
            && !(MicrogameController.instance.isDebugMode() && !MicrogameController.instance.DebugSettings.playMusic))
        {
            var session = MicrogameController.instance.session;
            var musicEndTime = AudioSettings.dspTime + (double)session.TimeRemaining;
            var musicPlayTime = musicEndTime - ((session.microgame.getDurationInBeats() - 1d) * Microgame.BeatLength);
            musicSource.PlayScheduled(musicPlayTime);
            if (!MicrogameController.instance.isDebugMode())
                musicSource.SetScheduledEndTime(musicEndTime);
        }
    }
}
