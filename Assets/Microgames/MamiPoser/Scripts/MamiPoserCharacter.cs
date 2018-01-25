using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserCharacter : MonoBehaviour {
    // This class is used for the initially spawned characters
    // (both the regular clones and the disguised Mamizou)
    // True form Mamizou is handled by the MamiPoserMamizou class

    [Header("Character features with regular/disguised variants")]
    public CharacterFeature[] characterFeatures;

    [Header("Sprite to hide when player chose wrong")]
    [SerializeField]
    private GameObject regularSprite;

    [Header("Sprite to display when player chose wrong")]
    [SerializeField]
    private GameObject wrongSprite;

    [Header("Sprite for looking straight")]
    [SerializeField]
    private GameObject lookingStraightSprite;

    [Header("Sprite for looking left")]
    [SerializeField]
    private GameObject lookingLeftSprite;

    [Header("Sprite for looking right")]
    [SerializeField]
    private GameObject lookingRightSprite;

    [SerializeField]
    private Collider2D clickCollider;

    public MamiPoserController controller;

    // Is this character Mamizou in disguise?
    public bool isDisguised { get; private set; }

    // Is this character showing an expression for being clicked incorrectly?
    public bool isChoseWrongExpression { get; private set; }

    void Start()
    {
        isChoseWrongExpression = false;
    }

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
        isChoseWrongExpression = true;

        if (regularSprite)
            regularSprite.SetActive(false);
        if (wrongSprite)
            wrongSprite.SetActive(true);
        // Also remove the eyes
        if (lookingStraightSprite)
            lookingStraightSprite.SetActive(false);
        if (lookingLeftSprite)
            lookingLeftSprite.SetActive(false);
        if (lookingRightSprite)
            lookingRightSprite.SetActive(false);
    }

    // Make the character look left
    public void LookLeft()
    {
        if (lookingStraightSprite)
            lookingStraightSprite.SetActive(false);
        if (lookingLeftSprite)
            lookingLeftSprite.SetActive(true);
        if (lookingRightSprite)
            lookingRightSprite.SetActive(false);
    }

    // Make the character look right
    public void LookRight()
    {
        if (lookingStraightSprite)
            lookingStraightSprite.SetActive(false);
        if (lookingLeftSprite)
            lookingLeftSprite.SetActive(false);
        if (lookingRightSprite)
            lookingRightSprite.SetActive(true);
    }

    // Check every frame if this character was clicked
    void Update()
    {
        if (MicrogameController.instance.getVictoryDetermined())
            return;
        if (!clickCollider)
            print("ERROR: MamiPoserCharacter: No clickCollider set!");
        if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
            Click();
    }

    // Handle clicks
    void Click()
    {
        if (!controller)
            print("ERROR: MamiPoserCharacter: No controller set!");
        controller.CharacterClicked(this);
    }
}
