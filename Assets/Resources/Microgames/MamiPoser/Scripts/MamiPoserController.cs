using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

public class MamiPoserController : MonoBehaviour {
    //[Header("The object containing the characters")]
    //public GameObject characterLibrary;

    [Header("Mamizou prefab")]
    [SerializeField]
    private MamiPoserMamizou mamizouPrefab;

    [Header("Other characters prefabs")]
    [SerializeField]
    private MamiPoserCharacter[] characterPrefabs;

    [Header("Smoke effect prefab")]
    [SerializeField]
    private GameObject smokePrefab;

    [Header("Number of character copies to be spawned")]
    [SerializeField]
    private int characterSpawnNumber;

    [Header("X coordinate bounds for spawned characters")]
    [SerializeField]
    private float leftBound = -20.0f/3, rightBound = 20.0f/3;

    [Header("Y coordinate for spawned characters")]
    [SerializeField]
    private float yCoordinate = -5f;

    // Randomly chosen character to be cloned
    private MamiPoserCharacter chosenCharacterPrefab;

    // Which of the clones is Mamizou?
    private int mamizouIndex;

    // All created cloned characters
    private List<MamiPoserCharacter> createdCharacters;

    // Mamizou object (true form)
    private MamiPoserMamizou mamizou;

    // Returns coordinates where the character with the given index should appear
    // Positions are on a horizontal line with equal distance between them
    private Vector2 CharacterPosition(int index)
    {
        print("X position for clone number " + index + ": " + (leftBound + (1 + 2 * index) * (rightBound - leftBound) / (2 * characterSpawnNumber)));
        return new Vector2(leftBound + (1 + 2 * index) * (rightBound - leftBound) / (2 * characterSpawnNumber), yCoordinate);
    }

    // Setup the microgame
    void Start()
    {
        // Determine which character to use and which of the copies is Mamizou
        chosenCharacterPrefab = characterPrefabs[Random.Range(0, characterPrefabs.Length)];
        print("Chosen character prefab: " + chosenCharacterPrefab);
        mamizouIndex = Random.Range(0, characterSpawnNumber);
        print("Mamizou index: " + mamizouIndex);

        // Spawn the characters
        createdCharacters = new List<MamiPoserCharacter>();
        for (int i = 0; i < characterSpawnNumber; i++)
        {
            MamiPoserCharacter newCharacter = Instantiate(chosenCharacterPrefab, CharacterPosition(i), Quaternion.identity);
            createdCharacters.Add(newCharacter);
            newCharacter.controller = this;
            if (i == mamizouIndex)
                // This one is disguised Mamizou
                newCharacter.SetDisguised();
            else
                // This one is not Mamizou
                newCharacter.SetRegular();
        }
    }

    // Handles a character being clicked
    public void CharacterClicked(MamiPoserCharacter clickedCharacter)
    {
        // Play the smoke effect
        Instantiate(smokePrefab, CharacterPosition(mamizouIndex), Quaternion.identity);
        // Spawn the real Mamizou - hidden for now
        mamizou = Instantiate(mamizouPrefab, CharacterPosition(mamizouIndex), Quaternion.identity);
        mamizou.gameObject.SetActive(false);

        // Determine if the player chose correctly
        if (clickedCharacter.isDisguised)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
            mamizou.ChoseRight();
        }
        else
        {
            MicrogameController.instance.setVictory(victory: false, final: true);
            mamizou.ChoseWrong();
            clickedCharacter.ChoseWrong();
        }

        // Make the other characters (except the one clicked) look at Mamizou
        for (int i = 0; i < characterSpawnNumber; i++)
        {
            if (createdCharacters[i] == clickedCharacter)
                continue;
            if (i < mamizouIndex)
                createdCharacters[i].LookRight();
            else if (i > mamizouIndex)
                createdCharacters[i].LookLeft();
        }
    }

    // Hide the disguised form Mamizou and show the true form Mamizou
    private void SwitchSpriteToMamizou()
    {
        createdCharacters[mamizouIndex].gameObject.SetActive(false);
        mamizou.gameObject.SetActive(true);
    }
}
