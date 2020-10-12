using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class SyncAnimatorSpeedToBeat : StageStateMachineBehaviour
    {
        [SerializeField]
        private SyncMode syncMode;

        private enum SyncMode
        {
            OneBeatPerSecond,
            TwoBeatsPerSecond
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            switch(syncMode)
            {
                case (SyncMode.OneBeatPerSecond):
                    Animator.speed = 1f / (float)Microgame.BeatLength;
                    return;
                case (SyncMode.TwoBeatsPerSecond):
                    Animator.speed = .5f / (float)Microgame.BeatLength;
                    return;

            }
        }

    }
}
