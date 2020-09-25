using StageFSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{


    class ApplyMicrogameFlagsFromStage : StageStateMachineBehaviour
    {
        [SerializeField]
        private string microgameIndexParam = "MicrogameIndex";
        [SerializeField]
        private string microgameVictoryParam = "MicrogameVictory";
        [SerializeField]
        private string lifeParam = "Life";

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var stage = assetToolbox.GetStage();
            var flags = stage.GetStateMachineFlags(
                Animator.GetInteger(microgameIndexParam),
                Animator.GetBool(microgameVictoryParam),
                Animator.GetInteger(lifeParam));
            foreach (var flag in flags)
            {
                Animator.SetBool(flag.Key, flag.Value);
            }
        }
    }
}
