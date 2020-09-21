using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class PlayAudioScheduled : StageStateMachineBehaviour
    {

        [SerializeField]
        string stateName;

        private AudioPlaybackController playbackController;
        private AudioSource audioSource;
        private AudioClip audioClip;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AssignToolboxAndAssets(animator, stateInfo, layerIndex);
            audioClip = assetToolbox.GetAssetGroupForState(stateName, Animator).GetAsset<AudioClip>();
            if (playbackController == null)
                playbackController = toolbox.GetTool<AudioPlaybackController>();

            audioSource = playbackController.GetNextAudioSource();
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (enterCondition == EnterCondition.Early && audioClip != null)
            {
                playbackController.ShiftAudioPlaybackTimeInBeats((double)animator.GetAnimatorTransitionInfo(layerIndex).duration);

                // Determine audio pitch in next state by factoring in speed changes at the end of last transition or the beginning of this one
                var speedController = toolbox.GetTool<SpeedController>();
                var speed = speedController.Speed;
                var currentSpeedChange = Utilities.GetBehaviour<ChangeSpeedLevel>(animator,
                    animator.GetNextAnimatorStateInfo(layerIndex));
                var previousSpeedChange = Utilities.GetBehaviour<ChangeSpeedLevel>(animator,
                    animator.GetCurrentAnimatorStateInfo(layerIndex));

                if (previousSpeedChange != null && previousSpeedChange.ApplySpeedChangeAtEnd)
                    speed = ChangeSpeedLevel.getChangedSpeed(speed, previousSpeedChange.SpeedChange);
                if (currentSpeedChange != null && !currentSpeedChange.ApplySpeedChangeAtEnd)
                    speed = ChangeSpeedLevel.getChangedSpeed(speed, currentSpeedChange.SpeedChange);

                playbackController.ScheduleClip(audioSource, audioClip, speedController.GetSpeedTimeScaleMult(speed));
            }
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (audioClip != null && enterCondition == EnterCondition.Normal)
            {
                playbackController.ResetAudioPlaybackTime();
                playbackController.ScheduleClip(audioSource, audioClip, toolbox.GetTool<SpeedController>().GetSpeedTimeScaleMult());
            }
        }
    }
}
