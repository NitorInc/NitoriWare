using StageFSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageFSM
{
    public class LoadFirstMicrogame : StageStateMachineBehaviour
    {
        [SerializeField]
        private string CallbackTriggerName = "FirstGameLoaded";

        MicrogamePlayer microgamePlayer;
        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();


            var session = assetToolbox.GetStage().getMicrogame(0).CreateSession();
            microgamePlayer = toolbox.GetTool<MicrogamePlayer>();
            microgamePlayer.EnqueueSession(session);

            toolbox.StartCoroutine(WaitForMicrogameLoad());
        }

        IEnumerator WaitForMicrogameLoad()
        {
            var holdTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            if (GameController.instance.sceneShifter.IsShifting)
                GameController.instance.sceneShifter.HoldFadeIn = true;
            while (!microgamePlayer.IsReadyToActivateScene())
            {
                yield return null;
            }
            if (GameController.instance.sceneShifter.IsShifting)
                GameController.instance.sceneShifter.HoldFadeIn = false;
            Time.timeScale = holdTimeScale;
            Animator.SetTrigger(CallbackTriggerName);
        }
    }
}