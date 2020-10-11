using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageFSM
{
    public class ApplySpeedLevel : StageStateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            toolbox.GetTool<SpeedController>().ApplySpeed();
        }

    }
}
