using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageFSM
{
    public class ChangeSpeedLevel : StageStateMachineBehaviour
    {
        [SerializeField]
        private SpeedChange speedChange;
        [SerializeField]
        private bool changeInTransition;
        [SerializeField]
        private string microgameIndexParam = "MicrogameIndex";

        public enum SpeedChange
        {
            None,
            SpeedUp,
            ResetToRoundStart,
            ResetToStageStart,
            ResetToOne
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (changeInTransition)
                ChangeSpeed();
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (!changeInTransition)
                ChangeSpeed();
        }

        void ChangeSpeed()
        {
            var speedController = toolbox.GetTool<SpeedController>();
            speedController.Speed = getChangedSpeed(speedController.Speed, speedChange);
        }

        public int getChangedSpeed(int speed, SpeedChange speedChange)
        {
            Stage stage;
            switch (speedChange)
            {
                case (SpeedChange.SpeedUp):
                    return Mathf.Clamp(speed + 1, 1, SpeedController.MaxSpeed);
                case (SpeedChange.ResetToOne):
                    return 1;
                case (SpeedChange.ResetToRoundStart):
                    stage = assetToolbox.GetStage();
                    var microgameIndex = Animator.GetInteger(microgameIndexParam);
                    var round = stage.GetRound(microgameIndex);
                    return stage.GetRoundSpeed(round);
                case (SpeedChange.ResetToStageStart):
                    stage = assetToolbox.GetStage();
                    return stage.getStartSpeed();
                default:
                    return speed;
            }
        }

    }
}
