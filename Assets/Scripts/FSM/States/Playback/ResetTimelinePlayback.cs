using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageFSM
{
    public class ResetTimelinePlayback : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            toolbox.GetTool<DirectorPlaybackController>().ResetPlayback();
        }
    }
}