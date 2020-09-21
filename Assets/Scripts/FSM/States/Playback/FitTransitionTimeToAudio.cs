using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace StageFSM
{
    public class FitTransitionTimeToAudio : StageStateMachineBehaviour
    {
        double dspStartTime;
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var playbackController = toolbox.GetTool<AudioPlaybackController>();
            playbackController = toolbox.GetTool<AudioPlaybackController>();
            dspStartTime = playbackController.LastScheduledAudioStartTime;
            Animator.speed = 0f;
        }

        public override void OnStateUpdateOfficial()
        {
            base.OnStateUpdateOfficial();

            if (Animator.IsInTransition(LayerIndex))
            {
                var transitionInfo = Animator.GetAnimatorTransitionInfo(LayerIndex);
                var realtimeTransitionDuration = (double)transitionInfo.duration * Microgame.BeatLength / Time.timeScale;
                var dspTimeToNextState = dspStartTime + realtimeTransitionDuration - AudioSettings.dspTime;
                if (dspTimeToNextState <= 0f)
                    Animator.speed = 1000f;
                else
                {
                    var beatsToState = (double)transitionInfo.duration * (1d - (double)transitionInfo.normalizedTime);
                    var scale = (float)(beatsToState / dspTimeToNextState);
                    Animator.speed = scale / Time.timeScale;
                }
            }
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            Animator.speed = 1f / (float)Microgame.BeatLength;
        }
    }
}