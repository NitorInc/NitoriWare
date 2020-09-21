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

        private float microgameStartTime;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();

            microgameStartTime = Time.time;
            var beatsToLastBeat = session.microgame.getDurationInBeats() - 1;
            eventListener.StartCoroutine(StartLastBeatIn(beatsToLastBeat * (float)Microgame.BeatLength));
        }

        protected override void OnMicrogameVictoryFinalized()
        {
            if (enableEndEarly && session.microgame.canEndEarly)
            {
                // End early if applicable
                var microgameBeatsLeft = ((invokedTime + 1f) - Time.time) / Microgame.BeatLength;
                var beatOffset = microgameBeatsLeft - endEarlyBeatThreshold;
                beatOffset -= beatOffset % 4f;
                if (beatOffset > 0f)
                {
                    // Microgame has over 4 + endEarlyBeatThreshold beats remaining, end early
                    var beatsToLastBeat = session.microgame.getDurationInBeats() - beatOffset - 1f;
                    var timeIntoMicrogame = Time.time - microgameStartTime;
                    var invokeIn = microgameStartTime + (beatsToLastBeat * Microgame.BeatLength) - Time.time;
                    MicrogameTimer.instance.CancelInvoke();
                    eventListener.StartCoroutine(StartLastBeatIn((float)invokeIn));

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