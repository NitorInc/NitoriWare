using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Playables;

namespace StageFSM
{
    public class TimelineFollowGameTime : StageStateMachineBehaviour
    {
        DirectorPlaybackController controller;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var playbackController = toolbox.GetTool<AudioPlaybackController>();
            controller = toolbox.GetTool<DirectorPlaybackController>();

            OnStateUpdateOfficial();
        }

        public override void OnStateUpdateOfficial()
        {
            base.OnStateUpdateOfficial();
            controller.time += Time.deltaTime;
        }
    }
}