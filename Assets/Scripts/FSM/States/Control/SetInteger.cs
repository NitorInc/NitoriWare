using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace StageFSM
{

    class SetInteger : StageStateMachineBehaviour
    {
        [SerializeField]
        private string IntegerName;
        [SerializeField]
        private int value;
        [SerializeField]
        private SetMode setMode;
        [SerializeField]
        private bool SetInTransition;

        public enum SetMode
        {
            Relative,
            Absolute
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            if (SetInTransition && enterCondition == EnterCondition.Early)
            {
                AssignToolboxAndAssets(animator, stateInfo, layerIndex);
                Apply();
            }
        }

        protected override void OnStateEnterOfficial()
        {
            if (!SetInTransition || enterCondition == EnterCondition.Normal)
                Apply();
            base.OnStateEnterOfficial();
        }

        void Apply()
        {
            int value = Animator.GetInteger(IntegerName);
            if (setMode == SetMode.Relative)
                value += this.value;
            else
                value = this.value;
            Animator.SetInteger(IntegerName, value);
        }
    }
}
