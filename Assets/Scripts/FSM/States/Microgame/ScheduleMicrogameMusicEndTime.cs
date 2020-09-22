using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class ScheduleMicrogameMusicEndTime : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var musicSource = toolbox.GetTool<AudioSource>();
            var session = toolbox.GetTool<MicrogamePlayer>().CurrentMicrogameSession;
            var speedController = toolbox.GetTool<SpeedController>();
            if (session.microgame.getDurationInBeats() < Mathf.Infinity && musicSource.clip != null && musicSource.isPlaying)
            {
                var totalBeats = session.microgame.getDurationInBeats() - 1d - session.EndEarlyBeats;
                var totalTime = totalBeats * Microgame.BeatLength;
                var currentTime = (double)musicSource.timeSamples / musicSource.clip.frequency;
                var timeRemaining = (totalTime - currentTime) / speedController.GetSpeedTimeScaleMult();
                Debug.Log(timeRemaining);
                musicSource.SetScheduledEndTime(AudioSettings.dspTime + timeRemaining);
            }
        }
    }
}
