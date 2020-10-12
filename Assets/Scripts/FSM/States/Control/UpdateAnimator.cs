using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class UpdateAnimator : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            Animator.Update(0f);
        }
    }
}