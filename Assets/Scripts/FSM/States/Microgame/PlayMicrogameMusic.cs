using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class PlayMicrogameMusic : StageStateMachineBehaviour
    {
        private AudioSource audioSource;

        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            audioSource = toolbox.GetTool<AudioSource>();
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            audioSource.clip = toolbox.GetTool<MicrogamePlayer>().CurrentMicrogameSession.GetMusicClip();
            audioSource.Play();
        }
    }
}
