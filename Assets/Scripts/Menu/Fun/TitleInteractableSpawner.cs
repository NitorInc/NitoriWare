using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleInteractableSpawner : MonoBehaviour
{
    public BoxCollider2D[] wallColliders;

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Vector2 spawnTimeBounds, spawnExtents;
    [SerializeField]
    private Interactable[] interactables;
#pragma warning restore 0649

    [System.Serializable]
    public struct Interactable
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnFrequency;
    }


    private float spawnIn;

	void Start()
	{
        spawnIn = MathHelper.randomRangeFromVector(spawnTimeBounds);
	}
	
	void Update()
	{
        if (!GameMenu.shifting && GameMenu.subMenu == GameMenu.SubMenu.Title)
            updateSpawner();
	}

    void updateSpawner()
    {
        spawnIn -= Time.deltaTime;
        if (spawnIn <= 0f)
        {
            spawnInteractable();
            spawnIn = MathHelper.randomRangeFromVector(spawnTimeBounds);
        }
    }

    void spawnInteractable()
    {
        float spawnWeight = 0f;
        foreach (Interactable interactable in interactables)
        {
            spawnWeight += interactable.spawnFrequency;
        }
        float spawnValue = Random.Range(0f, spawnWeight);
        GameObject spawn = null;
        foreach (Interactable interactable in interactables)
        {
            spawnValue -= interactable.spawnFrequency;
            if (spawnValue <= 0f)
            {
                spawn = interactable.prefab;
                break;
            }
        }

        GameObject newObject = Instantiate(spawn, transform);
        newObject.transform.localPosition = (Vector3)getSpawnOffset(Random.Range(0, 4));
        newObject.GetComponent<TitleFloatingInteractive>().spawner = this;
    }

    //0-3 are cardinal directions, starting at 
    Vector2 getSpawnOffset(int direction)
    {
        switch (direction)
        {
            case (0):
                return new Vector2(spawnExtents.x, Random.Range(-spawnExtents.y, spawnExtents.y));
            case (1):
                return new Vector2(Random.Range(-spawnExtents.x, spawnExtents.x), spawnExtents.y);
            case (2):
                return new Vector2(-spawnExtents.x, Random.Range(-spawnExtents.y, spawnExtents.y));
            case (3):
                return new Vector2(Random.Range(-spawnExtents.x, spawnExtents.x), -spawnExtents.y);
            default:
                return Vector2.zero;
        }
    }
}
