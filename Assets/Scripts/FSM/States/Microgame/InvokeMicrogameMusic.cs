using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class InvokeMicrogameMusic : StageStateMachineBehaviour
    {
        [SerializeField]
        private float inBeats = 1f;

        private AudioSource audioSource;
        private MicrogamePlayer microgamePlayer;

        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            audioSource = toolbox.GetTool<AudioSource>();
            microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            microgamePlayer.MicrogameEventListener.MicrogameStart.AddListener(SchedulePlayback);
        }

        void SchedulePlayback(Microgame.Session session)
        {
            audioSource.clip = toolbox.GetTool<MicrogamePlayer>().CurrentMicrogameSession.GetMusicClip();
            audioSource.PlayScheduled(AudioSettings.dspTime * 10000d);
            toolbox.StartCoroutine(PlayIn(inBeats * (float)Microgame.BeatLength));
        }

        IEnumerator PlayIn(float time)
        {
            yield return new WaitForSeconds(time);
            audioSource.SetScheduledStartTime(AudioSettings.dspTime);
        }
    }
}
