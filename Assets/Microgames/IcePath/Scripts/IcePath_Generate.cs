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
    [SerializeField] private GameObject prefabFog;
    [SerializeField] private GameObject prefabLamp;
    [SerializeField] private GameObject prefabLily;

    // Origin
    public static Vector2 origin;

    // The map to generate stuff with
    string map;

    // The grid for Cirno to walk around in
    public static string[,] tile = new string[9, 7];

    // Fog-related
    float fogTimer;
    float fogSpawnY;

    // Use this for initialization
	void Awake () {
        tileSize = spriteSize / 100;

        globalMapWidth = mapWidth;
        globalMapHeight = mapHeight;

        origin = transform.position;

        tile[8, 6] = ".";

        fogTimer = 1f;
        fogSpawnY = 0;

        // Read the map files
        int mapIndex = rand(1, 2);
        string path = "Assets/Microgames/IcePath/Maps/IcePath_Map" + mapDiff.ToString() + "0" + mapIndex.ToString() + ".txt";

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

        // Bring in a few fog
        Instantiate(prefabFog, new Vector2(rand(-12, 12), rand(-12, 12)), Quaternion.identity);
        Instantiate(prefabFog, new Vector2(rand(-12, 12), rand(-12, 12)), Quaternion.identity);
        Instantiate(prefabFog, new Vector2(rand(-12, 12), rand(-12, 12)), Quaternion.identity);
        Instantiate(prefabFog, new Vector2(rand(-12, 12), rand(-12, 12)), Quaternion.identity);
        Instantiate(prefabFog, new Vector2(rand(-12, 12), rand(-12, 12)), Quaternion.identity);

        // Debug
        print(map);
		
	}
	
	// Update is called once per frame
	void Update () {
        // Fog-related!!

        if (fogTimer > 0) {
            fogTimer -= Time.deltaTime;
        } else {
            Instantiate(prefabFog, new Vector2(24, rand(-12, 12)), Quaternion.identity);
            Instantiate(prefabFog, new Vector2(-24, rand(-12, 12)), Quaternion.identity);

            fogTimer = 3f;
            print("New mist spawned");
        }

    }

    int rand (float min, float max) {
        // Round random range
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
