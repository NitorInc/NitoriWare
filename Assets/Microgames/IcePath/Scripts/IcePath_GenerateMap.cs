using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_GenerateMap : MonoBehaviour {

    [Header("Map override (set to 0 for default random)")]
    [SerializeField] private int alwaysLoadMapNo;
    // for default random map, set this to 0

    [Header("Map data")]
    public int mapAmount;
    [SerializeField] private TextAsset no1;
    [SerializeField] private TextAsset no2;
    [SerializeField] private TextAsset no3;
    [SerializeField] private TextAsset no4;
    [SerializeField] private TextAsset no5;
    [SerializeField] private TextAsset no6;
    [SerializeField] private TextAsset no7;
    [SerializeField] private TextAsset no8;
    [SerializeField] private TextAsset no9;
    [SerializeField] private TextAsset no10;

    [Header("GameObject prefabs")]
    [SerializeField] private GameObject prefabCirno;
    [SerializeField] private GameObject prefabWaka;
    [SerializeField] private GameObject prefabIceCream;

    [SerializeField] private GameObject prefabIsleTile;
    [SerializeField] private GameObject prefabIceTile;
    [SerializeField] private GameObject prefabWakaNestTile;

    [SerializeField] private GameObject prefabLily;
    [SerializeField] private GameObject prefabRock;

    // Map related
    private TextAsset[]     textMap = new TextAsset[10];
    public static string[,] tile    = new string[13, 10];

    // Waka related (max 9 Waka per level)
    public static Vector2[] wakaStart   = new Vector2[9];
    public static Vector2[] wakaEnd     = new Vector2[9];
    public static Vector2[] wakaPass    = new Vector2[9];

    public static GameObject[] wakaObject = new GameObject[9];

    void Start() {
        // Assign stuff

        textMap[0] = no1;
        textMap[1] = no2;
        textMap[2] = no3;
        textMap[3] = no4;
        textMap[4] = no5;
        textMap[5] = no6;
        textMap[6] = no7;
        textMap[7] = no8;
        textMap[8] = no9;
        textMap[9] = no10;

        // Initiate all tiles
        tile[12, 9] = ".";

        // Read the map file
        int mapIndex = rand(0, mapAmount - 1);

        if (alwaysLoadMapNo != 0) {
            mapIndex = alwaysLoadMapNo - 1;
        }

        string map = textMap[mapIndex].text;

        // Indexers
        int finishIndex = 0;
        int wakaIndex   = 0;

        int finishRand;

        // Read map for number of finish tiles
        string n = map.Substring(0, 1);

        if (n == "0" || n == "1" || n == "2" || n == "3" || n == "4" ||
            n == "5" || n == "6" || n == "7" || n == "8" || n == "9") {
            int.TryParse(map.Substring(0, 1), out finishRand);
        } else {
            finishRand = 1;
        }

        int finishHere; // Definitive finish tile
        finishHere = rand(0, finishRand - 1);

        // Generate the map

        for (int yy = 0; yy < 9; yy++) {
            for (int xx = 0; xx < 12; xx++) {

                // Determine spawn position
                string readPos = map.Substring(12 * yy + xx + 2 * yy, 1); // Add 2 yy for new line keyword in string

                // Which tile to spawn?
                Quaternion quad = Quaternion.identity;

                GameObject waka;
                IcePath_Waka wakaScript;

                GameObject spawnObj;
                float randSize;

                switch (readPos) {
                    case "A": // Start isle tile
                        Instantiate(prefabIsleTile, mapPos(xx, -yy), quad);

                        GameObject cirno = Instantiate(prefabCirno, mapPos(xx, -yy), quad);
                        IcePath_Cirno cirnoScript = cirno.GetComponent<IcePath_Cirno>();

                        cirnoScript.cirnoGridX = xx;
                        cirnoScript.cirnoGridY = yy;
                        break;

                    case "B": // Finish isle tile
                        
                        // Is this the definitive finish tile?
                        if (finishIndex == finishHere) {
                            Instantiate(prefabIsleTile, mapPos(xx, -yy), quad);
                            Instantiate(prefabIceCream, mapPos(xx, -yy), quad);
                        } else {
                            Instantiate(prefabIsleTile, mapPos(xx, -yy), quad);
                        }
                        break;

                    case "#": // Ice tile
                        Instantiate(prefabIceTile, mapPos(xx, -yy), quad);
                        break;

                    case "@": // Isle tile
                        Instantiate(prefabIsleTile, mapPos(xx, -yy), quad);
                        break;

                    // Waka crossing tiles
                    case ">": // LEFT to RIGHT
                        Instantiate(prefabIceTile, mapPos(xx, -yy), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx - 1, -yy), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx + 1, -yy), quad);

                        wakaStart[wakaIndex] = mapPos(xx + 1, -yy);
                        wakaPass[wakaIndex] = mapPos(xx, -yy);
                        wakaEnd[wakaIndex] = mapPos(xx - 1, -yy);

                        waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                        wakaObject[wakaIndex] = waka;

                        waka.transform.Find("Rig").transform.localScale = new Vector3(-1, 1, 1);

                        wakaScript = waka.GetComponent<IcePath_Waka>();
                        wakaScript._wakaIndex = wakaIndex;
                        break;

                    case "<": // RIGHT to LEFT
                        Instantiate(prefabIceTile, mapPos(xx, -yy), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx - 1, -yy), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx + 1, -yy), quad);

                        wakaStart[wakaIndex] = mapPos(xx - 1, -yy);
                        wakaPass[wakaIndex] = mapPos(xx, -yy);
                        wakaEnd[wakaIndex] = mapPos(xx + 1, -yy);

                        waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                        wakaObject[wakaIndex] = waka;

                        wakaScript = waka.GetComponent<IcePath_Waka>();
                        wakaScript._wakaIndex = wakaIndex;
                        break;

                    case "^": // BOTTOM to TOP
                        Instantiate(prefabIceTile, mapPos(xx, -yy), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx, -(yy - 1)), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx, -(yy + 1)), quad);

                        wakaStart[wakaIndex] = mapPos(xx, -(yy - 1));
                        wakaPass[wakaIndex] = mapPos(xx, -yy);
                        wakaEnd[wakaIndex] = mapPos(xx, -(yy + 1));

                        waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                        wakaObject[wakaIndex] = waka;

                        wakaScript = waka.GetComponent<IcePath_Waka>();
                        wakaScript._wakaIndex = wakaIndex;
                        break;

                    case "v": // TOP to BOTTOM
                        Instantiate(prefabIceTile, mapPos(xx, -yy), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx, -(yy - 1)), quad);
                        Instantiate(prefabWakaNestTile, mapPos(xx, -(yy + 1)), quad);

                        wakaStart[wakaIndex] = mapPos(xx, -(yy + 1));
                        wakaPass[wakaIndex] = mapPos(xx, -yy);
                        wakaEnd[wakaIndex] = mapPos(xx, -(yy - 1));

                        waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                        wakaObject[wakaIndex] = waka;

                        waka.transform.Find("Rig").transform.localScale = new Vector3(-1, 1, 1);

                        wakaScript = waka.GetComponent<IcePath_Waka>();
                        wakaScript._wakaIndex = wakaIndex;
                        break;
                        
                    case "q": // Lily
                        Instantiate(prefabLily, mapPos(xx, -yy), quad);
                        break;

                    case "w": // Rock
                        Instantiate(prefabRock, mapPos(xx, -yy), quad);
                        break;

                    case ".": // Nothing
                        break;

                    default:
                        break;
                }
                // Write these down in the grid

                if (readPos == "<" ||
                    readPos == ">" ||
                    readPos == "^" ||
                    readPos == "v") {
                    // Write down Waka index if it is one
                    tile[xx, yy] = wakaIndex.ToString();
                    wakaIndex++;
                } else
                
                if (readPos == "B") {
                    // Write down the definitive finish tile
                    if (finishIndex == finishHere) {
                        tile[xx, yy] = "B";
                    } else {
                    // Write down the dummy finish tiles
                        tile[xx, yy] = "@";
                    }
                    finishIndex++;

                } else

                {
                    tile[xx, yy] = readPos;
                }

            }
        }

    }

    Vector2 mapPos(float posX, float posY) {
        return (new Vector2(-5.5f + posX, 4 + posY));
    }

    int rand(float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
