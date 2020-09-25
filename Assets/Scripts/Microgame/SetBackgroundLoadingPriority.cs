using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class SetBackgroundLoadingPriority : StageStateMachineBehaviour
    {
        [SerializeField]
        private ThreadPriority priority;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            Application.backgroundLoadingPriority = priority;
        }
    }
}
