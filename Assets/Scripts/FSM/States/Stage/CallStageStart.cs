using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    public class CallStageStart : StageStateMachineBehaviour
    {
        Stage stage;

        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            stage = toolbox.GetTool<StageControl>().Stage;
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            stage.onStageStart(null);
        }
    }
}
