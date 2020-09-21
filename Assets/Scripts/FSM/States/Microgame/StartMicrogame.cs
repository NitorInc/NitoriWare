using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StageFSM
{
    public class StartMicrogame : StageStateMachineBehaviour
    {
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            toolbox.GetTool<MicrogamePlayer>().StartCoroutine(MicrogameStartRoutine());
        }

        IEnumerator MicrogameStartRoutine()
        {
            var microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            if (!microgamePlayer.IsReadyToActivateScene())
            {
                var holdTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                AudioListener.pause = true;
                while (!microgamePlayer.IsReadyToActivateScene())
                {
                    yield return null;
                }
                AudioListener.pause = false;
                Time.timeScale = holdTimeScale;
            }
            microgamePlayer.ActivateScene();
        }
    }
}