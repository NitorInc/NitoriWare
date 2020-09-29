using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StageFSM
{
    public class StartMicrogame : StageStateMachineBehaviour
    {
        [SerializeField]
        private bool pauseAudioListenerUntilReady = false;
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
                if (pauseAudioListenerUntilReady)
                    AudioListener.pause = true;
                var holdPriority = Application.backgroundLoadingPriority;
                while (!microgamePlayer.IsReadyToActivateScene())
                {
                    Application.backgroundLoadingPriority = ThreadPriority.High;
                    yield return null;
                }
                if (pauseAudioListenerUntilReady)
                    AudioListener.pause = false;
                Application.backgroundLoadingPriority = holdPriority;
                Time.timeScale = holdTimeScale;
            }
            microgamePlayer.ActivateScene();
        }
    }
}