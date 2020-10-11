using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class HoldSceneFadeIn : StageStateMachineBehaviour
    {
        [SerializeField]
        private bool releaseOnExit = true;

        float holdTimeScale;
        private bool isHolding;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (GameController.instance.sceneShifter.IsShifting)
            {
                holdTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                GameController.instance.sceneShifter.HoldFadeIn = true;
                isHolding = true;
            }
            else
                isHolding = false;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            if (isHolding && releaseOnExit)
            {
                GameController.instance.sceneShifter.HoldFadeIn = false;
                Time.timeScale = holdTimeScale;
            }
        }
    }
}
