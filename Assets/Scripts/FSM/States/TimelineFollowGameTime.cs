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
        PlayableDirector director;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var playbackController = toolbox.GetTool<AudioPlaybackController>();
            director = toolbox.GetTool<PlayableDirector>();
        }

        public override void OnStateUpdateOfficial()
        {
            base.OnStateUpdateOfficial();
            director.time += Time.deltaTime;
        }
    }
}