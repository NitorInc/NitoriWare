using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserCharacter : MonoBehaviour {
    [Header("Character features with regular/disguised variants")]
    public CharacterFeature[] characterFeatures;

    public bool isDisguised { get; private set; }

    [System.Serializable]
    public class CharacterFeature
    {
        public GameObject regular;
        public GameObject disguised;
    }

    // Turn this character into a regular one (not Mamizou in disguise)
    public void SetRegular()
    {
        isDisguised = false;
        foreach (CharacterFeature feature in characterFeatures)
        {
            if (feature.regular)
                feature.regular.SetActive(true);
            if (feature.disguised)
                feature.disguised.SetActive(false);
        }
    }

    // Turn this character into Mamizou in disguise
    public void SetDisguised()
    {
        isDisguised = true;
        foreach (CharacterFeature feature in characterFeatures)
        {
            if (feature.regular)
                feature.regular.SetActive(false);
            if (feature.disguised)
                feature.disguised.SetActive(true);
        }
    }
}
