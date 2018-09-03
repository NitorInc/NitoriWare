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

    [Header("Eye pupils sprite")]
    [SerializeField]
    private GameObject pupilsSprite;

    [Header("Max distance the eye pupils move")]
    [SerializeField]
    private float pupilsMoveDistance = 0.03f;

    [Header("Mult affecting how far the cursor has to be for max pupil distance")]
    [SerializeField]
    private float pupilsMoveSoftnessMult = 1f;
    [SerializeField]
    private float pupilsMoveSoftnessExponent = .5f;

    [Header("Used to determine if cursor is within the sprite when clicked")]
    [SerializeField]
    private Collider2D[] clickColliders;

    public MamiPoserController controller;

    // Is this character Mamizou in disguise?
    public bool isDisguised { get; private set; }

    // Is this character showing an expression for being clicked incorrectly?
    public bool isChoseWrongExpression { get; private set; }

    // The position of the pupils when they're looking straight
    private Vector2 pupilsCenter;

    void Start()
    {
        isChoseWrongExpression = false;

        // Save the starting pupils position
        pupilsCenter = pupilsSprite.transform.position;
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
    }

    // Check every frame if this character was clicked
    void CheckClick()
    {
        if (clickColliders == null || clickColliders.Length == 0) {
            print("ERROR: MamiPoserCharacter: No clickColliders set!");
            return;
        }
        if (Input.GetMouseButtonDown(0))
            foreach (Collider2D collider in clickColliders)
                if (CameraHelper.isMouseOver(collider))
                {
                    Click();
                    return;
                }
    }

    // Move the pupils in the direction of the cursor
    void MovePupils()
    {
        Vector2 posDelta = (Vector2)CameraHelper.getCursorPosition() - pupilsCenter;

        float distance =
            Mathf.Pow((posDelta.magnitude / pupilsMoveSoftnessMult) / pupilsMoveDistance, pupilsMoveSoftnessExponent)
            * pupilsMoveDistance;
        posDelta = posDelta.resize(distance);

        if (posDelta.magnitude > pupilsMoveDistance)
            pupilsSprite.transform.position =
                pupilsCenter + posDelta.normalized * pupilsMoveDistance;
        else
            pupilsSprite.transform.position = pupilsCenter + posDelta;
    }

    void Update()
    {
        if (!MicrogameController.instance.getVictoryDetermined())
        {
            CheckClick();
            MovePupils();
        }
    }

    // Handle clicks
    void Click()
    {
        if (!controller)
            print("ERROR: MamiPoserCharacter: No controller set!");
        controller.CharacterClicked(this);
    }
}
