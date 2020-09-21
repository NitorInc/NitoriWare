using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    public class ScheduleResultAudioBoss : StageStateMachineBehaviour
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
                microgamePlayer.MicrogameEventListener.VictoryStatusFinalized.AddListener(VictoryFinalized);
            }

            var session = microgamePlayer.CurrentMicrogameSession;
            if (session.WasVictoryDetermined)
                VictoryFinalized(session);
        }

        void VictoryFinalized(Microgame.Session session)
        {
            if (!inStateOfficial)
                return;

            resultAudioPlayer.SetClip(session.VictoryStatus, assetToolbox.GetAssetGroupForState(VictoryAudioState, Animator).GetAsset<AudioClip>(), Time.timeScale);
            resultAudioPlayer.UpdateClipVolumes(session.VictoryStatus);
            var bossGame = session.microgame as MicrogameBoss;
            var endBeats = session.VictoryStatus ? bossGame.victoryEndBeats : bossGame.failureEndBeats;
            resultAudioPlayer.SchedulePlayback(endBeats * StageController.beatLength / Time.timeScale);
        }
    }
}
