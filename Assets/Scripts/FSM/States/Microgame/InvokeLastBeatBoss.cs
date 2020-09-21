using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageFSM
{
    class InvokeLastBeatBoss : InvokeLastBeat
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            // Boss microgame only ends after victory/loss is finalized
            invokedTime = float.PositiveInfinity;
        }

        protected override void OnMicrogameVictoryFinalized()
        {
            // Invoke end of boss game
            var bossGame = session.microgame as MicrogameBoss;
            var endBeats = session.VictoryStatus ? bossGame.victoryEndBeats : bossGame.failureEndBeats;
            eventListener.StartCoroutine(StartLastBeatIn((endBeats - 1f) * (float)Microgame.BeatLength));
        }
    }
}
