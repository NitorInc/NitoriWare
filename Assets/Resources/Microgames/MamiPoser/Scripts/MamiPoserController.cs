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
}
