using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_Generate : MonoBehaviour {

    [Header("Difficulty")]
    [SerializeField]
    private int mapDiff;

    [Header("Sprite size")]
    [SerializeField]
    private int spriteSize;
    public static float tileSize;
    // Tile size tells the generator the spacing between tiles in-scene

    [Header("Map width and height")]
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    public static int globalMapWidth;
    public static int globalMapHeight;

    [Header("Buncha prefabs")]
    [SerializeField] private GameObject prefabIsleTile;
    [SerializeField] private GameObject prefabIceTile;
    [SerializeField] private GameObject prefabCirno;
    [SerializeField] private GameObject prefabIceCream;
    [SerializeField] private GameObject prefabWaka;
    [SerializeField] private GameObject prefabLamp;
    [SerializeField] private GameObject prefabLily;

    // Origin
    public static Vector2 origin;

    // The map to generate stuff with
    string map;

    // The grid for Cirno to walk around in
    public static string[,] tile = new string[9, 7]; 
    
    // Use this for initialization
	void Awake () {
        tileSize = spriteSize / 100;

        globalMapWidth = mapWidth;
        globalMapHeight = mapHeight;

        origin = transform.position;

        tile[8, 6] = "0";

        // Read the map files
        int mapIndex = rand(1, 1);
        string path = "Assets/Microgames/IcePath/Maps/Map" + mapDiff.ToString() + "0" + mapIndex.ToString() + ".txt";

        StreamReader reader = new StreamReader(path, Encoding.UTF8);
        map = reader.ReadToEnd();

        // Generate the map!
        for (int i = 0; i < map.Length; i ++) {

            // Which tile to spawn?
            string pos = map.Substring(i, 1);
            int posX = i % mapWidth;
            int posY = Mathf.FloorToInt(i / mapWidth);

            Vector2 spawnPos = (Vector2)transform.position + tileSize * new Vector2(posX, -posY);

            switch (pos) {
                case "A": // Starting isle tile
                    Instantiate(prefabIsleTile, spawnPos, Quaternion.identity);
                    IcePath_Cirno.cirnoX = posX;
                    IcePath_Cirno.cirnoY = posY; break;
                case "B": // Finish isle tile
                    Instantiate(prefabIsleTile, spawnPos, Quaternion.identity);
                    Instantiate(prefabIceCream, spawnPos, Quaternion.identity); break;
                case "#": // Ice tile
                    Instantiate(prefabIceTile, spawnPos, Quaternion.identity); break;

                case "W": // Wakasagihime spawn
                    Instantiate(prefabWaka, spawnPos, Quaternion.identity); break;
                case "O": // Lamp
                    Instantiate(prefabLamp, spawnPos, Quaternion.identity); break;

                case "@": // Lilies
                    Instantiate(prefabLily, spawnPos, Quaternion.identity); break;

                default:
                    break;
            }
            // Write this down in the grid
            tile[posX, posY] = pos;

        }

        // Debug
        print(map);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    int rand (float min, float max) {
        // Round random range
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
