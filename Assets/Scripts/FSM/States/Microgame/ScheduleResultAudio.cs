using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class ScheduleResultAudio : StageStateMachineBehaviour
    {
        MicrogameResultAudioPlayer resultAudioPlayer;
        SpeedController speedController;

        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            resultAudioPlayer = toolbox.GetTool<MicrogameResultAudioPlayer>();
            speedController = toolbox.GetTool<SpeedController>();
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            resultAudioPlayer.SetPlaybackTime(AudioSettings.dspTime + (Microgame.BeatLength / speedController.GetSpeedTimeScaleMult()));
        }
    }
}
