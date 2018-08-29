using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_Master : MonoBehaviour {

    // Set the difficulty
    [Header("Difficulty")]
    [SerializeField]
    private int         mapDiff;
    public static int   globalMapDiff;

    // Map width and height for generation purposes
    [Header("Map width and height")]
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    public static int globalMapWidth;
    public static int globalMapHeight;

    // Tile size for spacing purposes
    [Header("Sprite size")]
    [SerializeField]
    private int         spriteSize;
    public static float globalTileSize;

    // Origin
    public static Vector2 globalOrigin;


    void Awake () {
        // Set global difficulty
        globalMapDiff = mapDiff;

        // Set global tile size
        globalTileSize = spriteSize / 100;

        // Set global map width and height
        globalMapWidth  = mapWidth;
        globalMapHeight = mapHeight;

        // Set global origin
        globalOrigin = transform.position;

	}

    int rand (float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
