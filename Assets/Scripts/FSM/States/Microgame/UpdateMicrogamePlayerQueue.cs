using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class UpdateMicrogamePlayerQueue : StageStateMachineBehaviour
    {
        [SerializeField]
        private string MicrogameIndexInt = "MicrogameIndex";
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var control = toolbox.GetTool<StageControl>();
            control.UpdateMicrogameQueue(Animator.GetInteger(MicrogameIndexInt));
        }
    }
}
