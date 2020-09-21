using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class BossCheck : StageStateMachineBehaviour
    {
        [SerializeField]
        private string boolName = "IsBoss";

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var session = toolbox.GetTool<MicrogamePlayer>().CurrentMicrogameSession;
            Animator.SetBool(boolName, session.microgame.isBossMicrogame());
        }
    }
}
