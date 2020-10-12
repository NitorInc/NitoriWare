using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageFSM
{
    class ReleaseSceneFadeIn : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (GameController.instance.sceneShifter.IsShifting)
                GameController.instance.sceneShifter.HoldFadeIn = false;
        }
    }
}
