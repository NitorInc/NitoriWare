using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserCharacter : MonoBehaviour {
    [Header("Character features with regular/disguised variants")]
    public CharacterFeature[] characterFeatures;

    [System.Serializable]
    public class CharacterFeature
    {
        public GameObject regular;
        public GameObject disguised;
    }
}
