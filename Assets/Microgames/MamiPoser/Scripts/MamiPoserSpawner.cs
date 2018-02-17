using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamiPoserSpawner : MonoBehaviour {
    [Header("Number of character copies to be spawned in this spawner")]
    [SerializeField]
    public int characterSpawnNumber;

    // Returns coordinates where the character with the given index should appear
    // Positions are on a horizontal line with equal distance between them
    private Vector2 CharacterPosition(int index)
    {
        Rect rect = GetComponent<RectTransform>().rect;
        print("X position for clone number " + index + ": " + (rect.xMin + (1 + 2 * index) * (rect.width) / (2 * characterSpawnNumber)));
        return new Vector2(rect.xMin + (1 + 2 * index) * (rect.width) / (2 * characterSpawnNumber), rect.y);
    }

    // Create parent objects to spawn characters in (for positioning)
    public List<Transform> CreateSlots()
    {
        List<Transform> slots = new List<Transform>();
        for (int i = 0; i < characterSpawnNumber; i++)
        {
            Transform newSlot = new GameObject().GetComponent<Transform>();
            newSlot.SetParent(GetComponent<RectTransform>());
            newSlot.localPosition = CharacterPosition(i);
            newSlot.rotation = GetComponent<Transform>().rotation;
            slots.Add(newSlot);
        }
        return slots;
    }
}
