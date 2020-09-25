using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace StageFSM
{
    class ResetAudioTime : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            toolbox.GetTool<AudioPlaybackController>().ResetAudioPlaybackTime();
            base.OnStateEnterOfficial();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!animator.IsInTransition(layerIndex))
                toolbox.GetTool<AudioPlaybackController>().ResetAudioPlaybackTime();
            base.OnStateUpdate(animator, StateInfo, LayerIndex);
        }
    }
}
