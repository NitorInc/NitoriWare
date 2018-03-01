using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.ClownTorch {
    // tagging without modifying global collision matrix nor tag manager
    public class ClownTorchTag : MonoBehaviour {

        public enum Type {
            Pyre,
            Water,
            PlayerTorch,
            ClownTorch
        }
        public Type type;
    }
}
