using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    public class PrepareResultAudioBoss : StageStateMachineBehaviour
    {
        [SerializeField]
        private string VictoryAudioState;
        [SerializeField]
        private string LossAudioState;

        MicrogamePlayer microgamePlayer;
        MicrogameResultAudioPlayer resultAudioPlayer;

        public override void OnStateInit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateInit(animator, stateInfo, layerIndex);
            resultAudioPlayer = toolbox.GetTool<MicrogameResultAudioPlayer>();
            microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            microgamePlayer.MicrogameEventListener.VictoryStatusFinalized.AddListener(VictoryFinalized);
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var microgamePlayer = toolbox.GetTool<MicrogamePlayer>();

            var session = microgamePlayer.CurrentMicrogameSession;
            if (session.WasVictoryDetermined)
                VictoryFinalized(session);
        }

        void VictoryFinalized(Microgame.Session session)
        {
            if (!inStateOfficial)
                return;

            var audioState = session.VictoryStatus ? VictoryAudioState : LossAudioState;
            resultAudioPlayer.SetClip(session.VictoryStatus, assetToolbox.GetAssetGroupForState(audioState).GetAsset<AudioClip>(), toolbox.GetTool<SpeedController>().GetSpeedTimeScaleMult());
            resultAudioPlayer.UpdateClipVolumes(session.VictoryStatus);
            resultAudioPlayer.SchedulePlayback(10000000d);
        }
    }
}
