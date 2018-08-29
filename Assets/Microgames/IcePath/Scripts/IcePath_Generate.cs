using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_Generate : MonoBehaviour {

    // Set the prefabs
    [Header("Buncha prefabs")]
    [SerializeField] private GameObject prefabIsleTile;
    [SerializeField] private GameObject prefabIceTile;
    [SerializeField] private GameObject prefabCirno;
    [SerializeField] private GameObject prefabIceCream;
    [SerializeField] private GameObject prefabWaka;
    [SerializeField] private GameObject prefabFog;
    [SerializeField] private GameObject prefabLamp;
    [SerializeField] private GameObject prefabLily;

    // Map stuff
    public static string[,] globalTile = new string[10, 10];
    public static Vector2   globalWakaStart, globalWakaEnd;

    // Bring in static variables
    int mapDiff;
    int mapWidth;
    int mapHeight;
    float tileSize;
    Vector2 origin;


    void Awake () {
        // Assign static variables
        mapDiff     = IcePath_Master.globalMapDiff;
        mapWidth    = IcePath_Master.globalMapWidth;
        mapHeight   = IcePath_Master.globalMapHeight;
        tileSize    = IcePath_Master.globalTileSize;
        origin      = IcePath_Master.globalOrigin;

        // Initiate all tiles
        globalTile[mapWidth, mapHeight] = ".";

        // Read the map file
        // Index is selected randomly
        int     mapIndex = rand(1, 3);
        string  mapPath = "Assets/Microgames/IcePath/Maps/IcePath_Map" + mapDiff.ToString() + "0" + mapIndex.ToString() + ".txt";

        StreamReader mapReader = new StreamReader(mapPath, Encoding.UTF8);
        string map = mapReader.ReadToEnd();

        // Generate the map!
        for (int i = 0; i < map.Length; i ++) {
            
            // Which tile to spawn?
            string readPos = map.Substring(i, 1);

            int tilePosX = i % mapWidth;
            int tilePosY = Mathf.FloorToInt(i / mapWidth);

            Vector2 spawnPos = origin + tileSize * new Vector2(tilePosX, -tilePosY);

            Quaternion quad = Quaternion.identity;
            switch (readPos) {
                case "A": // Start isle tile
                    Instantiate(prefabIsleTile, spawnPos, quad);
                    
                    IcePath_Cirno.cirnoGridX = tilePosX;
                    IcePath_Cirno.cirnoGridY = tilePosY;
                    break;

                case "B": // Finish isle tile
                    Instantiate(prefabIsleTile, spawnPos, quad);
                    Instantiate(prefabIceCream, spawnPos, quad);
                    break;

                case "#": // Ice tile
                    Instantiate(prefabIceTile,  spawnPos, quad);
                    break;

                case "a": // Wakasagihime start tile
                    Instantiate(prefabWaka,     spawnPos, quad);

                    globalWakaStart = spawnPos;
                    break;

                case "b": // Wakasagihime passing tile
                    Instantiate(prefabIceTile,  spawnPos, quad);
                    break;

                case "c": // Wakasagihime end tile
                    globalWakaEnd =   spawnPos;
                    break;

                case "O": // Lamp
                    Instantiate(prefabLamp, spawnPos, quad);
                    break;

                case "x": // Lilies
                    Instantiate(prefabLily, spawnPos, quad);
                    break;

                case ".": // Nothing
                    break;

                default:
                    break;
            }
            // Write this down in the grid
            globalTile[tilePosX, tilePosY] = readPos;
        }
		
	}

    int rand (float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
