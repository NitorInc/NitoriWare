using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class ScheduleResultAudioNormal : StageStateMachineBehaviour
    {
        [SerializeField]
        private string VictoryAudioState;
        [SerializeField]
        private string LossAudioState;

        private bool subscribedToEvent;
        MicrogameResultAudioPlayer resultAudioPlayer;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            if (!subscribedToEvent)
            {
                resultAudioPlayer = toolbox.GetTool<MicrogameResultAudioPlayer>();
                microgamePlayer.MicrogameEventListener.VictoryStatusUpdated.AddListener(UpdateVictory);
            }
            resultAudioPlayer.SetClip(true, assetToolbox.GetAssetGroupForState(VictoryAudioState, Animator).GetAsset<AudioClip>(), Time.timeScale);
            resultAudioPlayer.SetClip(false, assetToolbox.GetAssetGroupForState(LossAudioState, Animator).GetAsset<AudioClip>(), Time.timeScale);

            var session = microgamePlayer.CurrentMicrogameSession;
            var timeRemaining = (double)microgamePlayer.CurrentMicrogameSession.TimeRemaining;
            resultAudioPlayer.SchedulePlayback(timeRemaining / (double)Time.timeScale);
            UpdateVictory(session);
        }

        void UpdateVictory(Microgame.Session session)
        {
            resultAudioPlayer.UpdateClipVolumes(session.VictoryStatus);
        }
    }
}
