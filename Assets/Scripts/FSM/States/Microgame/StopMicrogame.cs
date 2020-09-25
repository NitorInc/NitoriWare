using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageFSM
{
    class StopMicrogame : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            microgamePlayer.StopMicrogame();
        }
    }
}
