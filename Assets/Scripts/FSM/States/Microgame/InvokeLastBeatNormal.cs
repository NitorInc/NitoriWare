using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace StageFSM
{
    public class InvokeLastBeatNormal : InvokeLastBeat
    {
        [SerializeField]
        private bool enableEndEarly = true;
        [SerializeField]
        private float endEarlyBeatThreshold = 2f;
        [SerializeField]
        private bool rescheduleResultAudio = true;

        private float scheduledEndTime;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();

            scheduledEndTime = session.EndTime;
            eventListener.StartCoroutine(StartLastBeatIn(scheduledEndTime - (float)Microgame.BeatLength - Time.time));
        }

        protected override void OnMicrogameVictoryFinalized()
        {
            if (enableEndEarly && session.microgame.canEndEarly)
            {
                // End early if applicable
                var microgameBeatsLeft = session.BeatsRemaining;
                var beatOffset = microgameBeatsLeft - endEarlyBeatThreshold;
                beatOffset -= beatOffset % 4f;
                if (beatOffset > 0f)
                {
                    // Microgame has over 4 + endEarlyBeatThreshold beats remaining, end early
                    session.IsEndingEarly = true;
                    scheduledEndTime -= beatOffset * (float)Microgame.BeatLength;
                    eventListener.StartCoroutine(StartLastBeatIn(scheduledEndTime - (float)Microgame.BeatLength - Time.time));

                    if (rescheduleResultAudio)
                    {
                        var player = toolbox.GetTool<MicrogameResultAudioPlayer>();
                        player.ShiftPlaybackTime((double)-beatOffset * Microgame.BeatLength / Time.timeScale);
                    }
                }
            }
        }
    }
}