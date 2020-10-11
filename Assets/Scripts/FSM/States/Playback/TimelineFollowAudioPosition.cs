using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Playables;

namespace StageFSM
{
    public class TimelineFollowAudioPosition : StageStateMachineBehaviour
    {
        AudioSource audioSource;
        DirectorPlaybackController controller;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var playbackController = toolbox.GetTool<AudioPlaybackController>();
            controller = toolbox.GetTool<DirectorPlaybackController>();
            audioSource = playbackController.CurrentSource;
        }

        public override void OnStateUpdateOfficial()
        {
            base.OnStateUpdateOfficial();
            if (inStateOfficial)
            {
                controller.time = audioSource.isPlaying ? audioSource.time : audioSource.clip.length;
            }
        }
    }
}