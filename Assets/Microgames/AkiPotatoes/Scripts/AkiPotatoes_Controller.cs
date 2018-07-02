using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AkiPotatoes
{
    public class AkiPotatoes_Controller : MonoBehaviour
    {
        public static AkiPotatoes_Controller singleton = null;

        [Header("Difficulty")]
        [SerializeField]
        private int CookedPotatoesCurrent = 0;
        [SerializeField]
        private int CookedPotatoesRequirement = 1;

        void Start()
        {
            singleton = this;

            CookedPotatoesCurrent = 0;
        }


        public void AddCookedPotato()
        {
            CookedPotatoesCurrent++;

            if (CookedPotatoesCurrent >= CookedPotatoesRequirement)
            {
                MicrogameController.instance.setVictory(true);
            }
        }

    }

}