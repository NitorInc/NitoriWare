using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageFSM
{
    public class ChangeSpeedLevel : StageStateMachineBehaviour
    {
        [SerializeField]
        private Stage.Interruption.SpeedChange speedChange;
        public Stage.Interruption.SpeedChange SpeedChange => speedChange;
        [SerializeField]
        private bool applySpeedChangeAtEnd;
        public bool ApplySpeedChangeAtEnd => applySpeedChangeAtEnd;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (!applySpeedChangeAtEnd && speedChange != Stage.Interruption.SpeedChange.None)
                ApplySpeedChange();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, StateInfo, layerIndex);
            if (applySpeedChangeAtEnd && speedChange != Stage.Interruption.SpeedChange.None)
                ApplySpeedChange();
        }

        void ApplySpeedChange()
        {
            var speedController = toolbox.GetTool<SpeedController>();
            speedController.Speed = getChangedSpeed(speedController.Speed, speedChange);
            speedController.ApplySpeed();
        }

        public static int getChangedSpeed(int speed, Stage.Interruption.SpeedChange speedChange)
        {
            switch (speedChange)
            {
                case (Stage.Interruption.SpeedChange.SpeedUp):
                    return Mathf.Clamp(speed + 1, 1, SpeedController.MAX_SPEED);
                case (Stage.Interruption.SpeedChange.ResetSpeed):
                    return 1;
                //case (Stage.Interruption.SpeedChange.Custom):
                //    return Mathf.Clamp(stage.getCustomSpeed(microgameCount, interruption), 1, SpeedController.MAX_SPEED);
                default:
                    return speed;
            }
        }

    }
}
