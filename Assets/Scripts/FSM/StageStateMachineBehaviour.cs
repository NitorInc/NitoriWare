﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace StageFSM
{
    public class StageStateMachineBehaviour : StateMachineBehaviour
    {
        protected FSMToolbox toolbox;
        protected FSMStateAssets assetToolbox;
        public Animator Animator { get; private set; }
        public AnimatorStateInfo StateInfo { get; private set; }
        public int LayerIndex { get; private set; }

        protected bool inStateRaw = false;
        protected bool inStateOfficial = false;
        protected EnterCondition enterCondition;
        protected enum EnterCondition
        {
            Normal,
            Early
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Animator = animator;
            StateInfo = stateInfo;
            LayerIndex = layerIndex;
            inStateRaw = true;

            AssignToolboxAndAssets(animator);

            base.OnStateEnter(animator, stateInfo, layerIndex);

            if (animator.IsInTransition(layerIndex) && animator.GetCurrentAnimatorStateInfo(layerIndex).fullPathHash != StateInfo.fullPathHash)
            {
                // This state was reached early because it is being transitioned to
                // We do this to peek at the next state and invoke the functions/music at the approrpriate time
                enterCondition = EnterCondition.Early;
            }
            else
            {
                // We are at the correct time for state activation, reset controller's audio playback timer as well
                enterCondition = EnterCondition.Normal;
                OnStateEnterOfficial();
            }
        }

        // This function is separated in case a subclass needs to access these before calling base.OnStateEnter
        protected void AssignToolboxAndAssets(Animator animator)
        {
            if (toolbox == null)
            {
                toolbox = animator.GetComponent<FSMToolbox>();
                if (toolbox != null)
                    assetToolbox = toolbox.GetTool<FSMStateAssets>();
            }
            
        }

        /// <summary>
        /// Override this in subclasses for when the state actually starts
        /// </summary>
        protected virtual void OnStateEnterOfficial()
        {
            inStateOfficial = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (inStateOfficial)
                OnStateUpdateOfficial();
        }

        public virtual void OnStateUpdateOfficial()
        {

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            inStateRaw = false;
            inStateOfficial = false;

            // If next state was entered early, enter the behaviours officially now
            var nextStateBehaviours = Utilities.GetBehaviours<StageStateMachineBehaviour>(animator, animator.GetCurrentAnimatorStateInfo(layerIndex));
            foreach (var behaviour in nextStateBehaviours)
            {
                if (behaviour != this
                    && behaviour.inStateRaw
                    && !behaviour.inStateOfficial
                    && behaviour.enterCondition == EnterCondition.Early)
                    behaviour.OnStateEnterOfficial();
            }
        }

        float GetTransitionTimeRemaining(Animator animator, int layerIndex)
        {
            if (!animator.IsInTransition(layerIndex))
                return 0f;

            var transitionData = animator.GetAnimatorTransitionInfo(layerIndex);
            var timeRemaining = transitionData.duration * (1f - transitionData.normalizedTime);
            timeRemaining /= animator.speed;
            return timeRemaining;
        }
    }

    public static class Utilities
    {
        public static T GetBehaviour<T>(this Animator animator, AnimatorStateInfo stateInfo) where T : StageStateMachineBehaviour

        {
            return animator.GetBehaviours<T>().ToList()
                .FirstOrDefault(behaviour => behaviour.StateInfo.fullPathHash == stateInfo.fullPathHash);
        }
        public static T[] GetBehaviours<T>(this Animator animator, AnimatorStateInfo stateInfo) where T : StageStateMachineBehaviour

        {
            return animator.GetBehaviours<T>()
                .Where(behaviour => behaviour.StateInfo.fullPathHash == stateInfo.fullPathHash)
                .ToArray();
        }
    }
}
