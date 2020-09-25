using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class PrepareResultAudioNormal : StageStateMachineBehaviour
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
            microgamePlayer.MicrogameEventListener.VictoryStatusUpdated.AddListener(UpdateVictory);
        }

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            resultAudioPlayer.SetClip(true, assetToolbox.GetAssetGroupForState(VictoryAudioState).GetAsset<AudioClip>(), Time.timeScale);
            resultAudioPlayer.SetClip(false, assetToolbox.GetAssetGroupForState(LossAudioState).GetAsset<AudioClip>(), Time.timeScale);

            resultAudioPlayer.SchedulePlayback(1000000d);
            UpdateVictory(microgamePlayer.CurrentMicrogameSession);
        }

        void UpdateVictory(Microgame.Session session)
        {
            resultAudioPlayer.UpdateClipVolumes(session.VictoryStatus);
        }
    }
}
