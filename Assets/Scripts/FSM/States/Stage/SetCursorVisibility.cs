using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class SetCursorVisibility : StageStateMachineBehaviour
    {
        [SerializeField]
        private bool visible;

        private CursorVisibilityController controller;
        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            controller = toolbox.GetTool<CursorVisibilityController>();
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            controller.SetVisibility(visible);
        }
    }
}
