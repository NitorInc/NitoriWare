using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class ListenForVictory : StageStateMachineBehaviour
    {
        [SerializeField]
        private string victoryBool = "MicrogameVictory";

        MicrogamePlayer microgamePlayer;

        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            microgamePlayer.MicrogameEventListener.VictoryStatusUpdated.AddListener(UpdateVictory);
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            UpdateVictory(microgamePlayer.CurrentMicrogameSession);
        }

        void UpdateVictory(Microgame.Session session)
        {
            Animator.SetBool(victoryBool, session.VictoryStatus);
        }
    }
}
