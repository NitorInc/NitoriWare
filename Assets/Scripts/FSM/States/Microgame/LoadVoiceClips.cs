using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageFSM
{
    class LoadVoiceClips : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var voicePlayer = toolbox.GetTool<VoicePlayer>();
            var stage = toolbox.GetTool<StageControl>().Stage;
            voicePlayer.loadClips(stage.getVoiceSet());
        }
    }
}
