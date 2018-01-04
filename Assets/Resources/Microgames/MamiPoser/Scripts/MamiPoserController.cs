using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

public class MamiPoserController : MonoBehaviour {
    //[Header("The object containing the characters")]
    //public GameObject characterLibrary;

    [Header("Mamizou prefab")]
    public MamiPoserMamizou mamizouPrefab;

    [Header("Other characters prefabs")]
    public MamiPoserCharacter[] characterPrefabs;

    [Header("Smoke effect prefab")]
    public GameObject smokePrefab;

    [Header("Number of character copies to be spawned")]
    public int characterSpawnNumber;

    [Header("X coordinate bounds for spawned characters")]
    public float leftBound = -20.0f/3, rightBound = 20.0f/3;

    [Header("Y coordinate for spawned characters")]
    public float yCoordinate = -5f;

    [Header("Delay before Mamizou's sprite appears after clicking")]
    public float mamizouAppearDelay = 0f;

    private MamiPoserCharacter chosenCharacterPrefab;
    private int mamizouIndex;
    private List<MamiPoserCharacter> createdCharacters;
    private Timer mamizouAppearTimer;
    private MamiPoserMamizou mamizou;

    private Vector2 CharacterPosition(int index)
    {
        print("X position for clone number " + index + ": " + (leftBound + (1 + 2 * index) * (rightBound - leftBound) / (2 * characterSpawnNumber)));
        return new Vector2(leftBound + (1 + 2 * index) * (rightBound - leftBound) / (2 * characterSpawnNumber), yCoordinate);
    }

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
                newCharacter.SetDisguised();
            else
                newCharacter.SetRegular();
        }
    }

    public void CharacterClicked(MamiPoserCharacter clickedCharacter)
    {
        Instantiate(smokePrefab, CharacterPosition(mamizouIndex), Quaternion.identity);
        mamizou = Instantiate(mamizouPrefab, CharacterPosition(mamizouIndex), Quaternion.identity);
        mamizou.gameObject.SetActive(false);
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
        mamizouAppearTimer = TimerManager.NewTimer(mamizouAppearDelay, SwitchSpriteToMamizou, 0);
    }

    private void SwitchSpriteToMamizou()
    {
        createdCharacters[mamizouIndex].gameObject.SetActive(false);
        mamizou.gameObject.SetActive(true);
    }
}
