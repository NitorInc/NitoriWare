using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spawn : MonoBehaviour
{
    // Reference to the Prefab. Drag a Prefab into this field in the Inspector.
    public GameObject crowd;
    public int difficulty;
    Collider2D zone;

    // This script will simply instantiate the Prefab when the game starts.
    void Start()
    {
        zone = GetComponent<Collider2D>();

        Bounds bounds = zone.bounds;
        

        for (int i = 0; i <= difficulty; i++)
        {
            Instantiate(crowd, new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0), Quaternion.identity);
            Instantiate(crowd, new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), 0), Quaternion.identity);
        }
    }
}