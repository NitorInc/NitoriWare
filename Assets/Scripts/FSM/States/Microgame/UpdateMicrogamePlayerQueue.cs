using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class UpdateMicrogamePlayerQueue : StageStateMachineBehaviour
    {
        [SerializeField]
        private int maxPreloadedQueue = 3;
        [SerializeField]
        private string MicrogameIndexInt = "MicrogameIndex";

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();

            var microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            var microgameIndex = Animator.GetInteger(MicrogameIndexInt);

            //Queue all available, unqueued microgames, and make sure at least one is queued
            var queueCount = microgamePlayer.QueuedMicrogameCount();
            int index = microgameIndex + queueCount;
            var stage = assetToolbox.GetStage();
            while (queueCount == 0 || (queueCount < maxPreloadedQueue && stage.isMicrogameDetermined(index)))
            {
                var session = stage.getMicrogame(index).CreateSession();
                microgamePlayer.EnqueueSession(session);
                queueCount++;
                index++;
            }
        }
    }
}
