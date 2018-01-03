using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserCharacter : MonoBehaviour {
    [Header("Character features with regular/disguised variants")]
    public CharacterFeature[] characterFeatures;

    [Header("Sprite to hide when player chose wrong")]
    [SerializeField]
    private GameObject regularSprite;

    [Header("Sprite to display when player chose wrong")]
    [SerializeField]
    private GameObject wrongSprite;

    [SerializeField]
    private Collider2D clickCollider;

    public MamiPoserController controller;

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

    // Change the sprite into "chose wrong" expression
    public void ChoseWrong()
    {
        if (regularSprite)
            regularSprite.SetActive(false);
        if (wrongSprite)
            wrongSprite.SetActive(true);
    }

    void Update()
    {
        if (!clickCollider)
            print("ERROR: MamiPoserCharacter: No clickCollider set!");
        if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
            Click();
    }

    void Click()
    {
        if (!controller)
            print("ERROR: MamiPoserCharacter: No controller set!");
        controller.CharacterClicked(this);
    }
}
