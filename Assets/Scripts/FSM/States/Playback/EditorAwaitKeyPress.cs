using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace StageFSM
{
    class EditorAwaitKeyPress : StageStateMachineBehaviour
    {
        [SerializeField]
        private KeyCode advanceKey = KeyCode.Space;
        [SerializeField]
        private string advanceTrigger = "Advance";
        [SerializeField]
        private bool bypassIfNotStartScene = true;

        bool Bypass => !Application.isEditor
            || (bypassIfNotStartScene
                && !GameController.instance.getStartScene().Equals(Animator.gameObject.scene.name));

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            if (Bypass)
            {
                Animator.SetTrigger(advanceTrigger);
                Animator.Update(0f);
            }
        }

        public override void OnStateUpdateOfficial()
        {
            base.OnStateUpdateOfficial();
            if (!Bypass && Input.GetKeyDown(advanceKey))
                Animator.SetTrigger(advanceTrigger);
        }
    }
}
