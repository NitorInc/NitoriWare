using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class GameOver : StageStateMachineBehaviour
    {
        [SerializeField]
        private string callbackTrigger = "Advance";

        StageGameOverMenu gameoverMenu;

        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            gameoverMenu = toolbox.GetTool<StageGameOverMenu>();
            gameoverMenu.onRetry.AddListener(Retry);
        }

        protected override void OnStateEnterOfficial()
        {
            gameoverMenu.Trigger(2);    // TODO score
        }

        void Retry()
        {
            Animator.SetTrigger(callbackTrigger);
        }
    }
}
