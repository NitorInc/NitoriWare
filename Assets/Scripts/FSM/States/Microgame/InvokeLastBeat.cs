using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


namespace StageFSM
{
    public abstract class InvokeLastBeat : StageStateMachineBehaviour
    {
        [SerializeField]
        private string triggerName = "LastBeat";

        bool subscribedToEvents;
        bool lastBeatReached;
        protected MicrogameEventListener eventListener;
        protected float invokedTime;
        protected Microgame.Session session;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (!subscribedToEvents)
            {
                eventListener = toolbox.GetTool<MicrogamePlayer>().MicrogameEventListener;
                eventListener.VictoryStatusFinalized.AddListener(MicrogameVictoryFinalized);
                subscribedToEvents = true;
            }
            lastBeatReached = false;

            session = toolbox.GetTool<MicrogamePlayer>().CurrentMicrogameSession;
            // Check if victory was determined too early
            if (session.WasVictoryDetermined)
                OnMicrogameVictoryFinalized();
        }


        void MicrogameVictoryFinalized(Microgame.Session session)
        {
            // Don't allow us to get here early
            if (inStateOfficial)
                OnMicrogameVictoryFinalized();
        }

        protected abstract void OnMicrogameVictoryFinalized();

        protected virtual IEnumerator StartLastBeatIn(float time)
        {
            invokedTime = Time.time + time;
            yield return new WaitForSeconds(time);
            if (!lastBeatReached)
            {
                lastBeatReached = true;
                Animator.SetTrigger(triggerName);
            }
        }
    }
}