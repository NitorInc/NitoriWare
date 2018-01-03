using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserController : MonoBehaviour {
    //[Header("The object containing the characters")]
    //public GameObject characterLibrary;

    [Header("Mamizou prefab")]
    public MamiPoserMamizou mamizouPrefab;

    [Header("Other characters prefabs")]
    public MamiPoserCharacter[] characterPrefabs;

    [Header("Number of character copies to be spawned")]
    public int characterSpawnNumber;

    private MamiPoserCharacter chosenCharacterPrefab;
    private int mamizouIndex;
    private List<MamiPoserCharacter> createdCharacters;

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
            createdCharacters.Add(Instantiate(chosenCharacterPrefab));
        }
    }
}
