using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

namespace NitorInc.NueAbduct
{
    public class NueAbductController : MonoBehaviour
    {
        [Header("All animals that need to be sucked up")]
        [SerializeField]
        private NueAbductVictimBehavior[] animals;

        public Animator victoryAnimator;

        // Check victory condition (all animals sucked)
        void Update() {
            foreach (var animal in animals)
                if (animal.currState != NueAbductVictimBehavior.State.Sucked)
                    return;
            // No animals left, we win
            MicrogameController.instance.setVictory(victory: true, final: true);
            victoryAnimator.Play("Nyoom");
            enabled = false;
        }
    }
}