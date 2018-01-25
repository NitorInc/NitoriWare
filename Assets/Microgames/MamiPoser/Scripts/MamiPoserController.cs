﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NitorInc.Utility;

public class MamiPoserController : MonoBehaviour
{
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

  [Header("Spawners to spawn characters in")]
  [SerializeField]
  private MamiPoserSpawner[] spawners;

  [Header("Delay before Mamizou's sprite appears after clicking")]
  [SerializeField]
  private float mamizouAppearDelay = 0f;

  // Number of character copies to be spawned
  private int characterSpawnNumber = 0;

  // Randomly chosen character to be cloned
  private MamiPoserCharacter chosenCharacterPrefab;

  // Which of the clones is Mamizou?
  private int mamizouIndex;

  // Parent objects for created characters (used for positioning)
  private List<Transform> characterSlots;

  // All created cloned characters
  private List<MamiPoserCharacter> createdCharacters;

  // Mamizou object (true form)
  private MamiPoserMamizou mamizou;

  // Setup the microgame
  void Start()
  {
    // Calculate how many characters to spawn by adding numbers from spawners
    // and create slots to spawn them in
    characterSlots = new List<Transform>();
    foreach (MamiPoserSpawner spawner in spawners)
    {
      characterSpawnNumber += spawner.characterSpawnNumber;
      characterSlots.AddRange(spawner.CreateSlots());
    }

    // Determine which character to use and which of the copies is Mamizou
    chosenCharacterPrefab = characterPrefabs[Random.Range(0, characterPrefabs.Length)];
    print("Chosen character prefab: " + chosenCharacterPrefab);
    mamizouIndex = Random.Range(0, characterSpawnNumber);
    print("Mamizou index: " + mamizouIndex);

    // Spawn the characters
    createdCharacters = new List<MamiPoserCharacter>();
    for (int i = 0; i < characterSpawnNumber; i++)
    {
      MamiPoserCharacter newCharacter = Instantiate(chosenCharacterPrefab, Vector2.zero, Quaternion.identity);
      newCharacter.GetComponent<Transform>().SetParent(characterSlots[i], false);
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
    GameObject smoke = Instantiate(smokePrefab, Vector2.zero, Quaternion.identity);
    smoke.GetComponent<Transform>().SetParent(characterSlots[mamizouIndex], false);
    // Spawn the real Mamizou - hidden for now
    mamizou = Instantiate(mamizouPrefab, Vector2.zero, Quaternion.identity);
    mamizou.GetComponent<Transform>().SetParent(characterSlots[mamizouIndex], false);
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
