using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_GenerateMap : MonoBehaviour {

    [Header("Map override (set to 0 for default random)")]
    [SerializeField] private int alwaysLoadMapNo;
    // for default random map, set this to 0

    [Header("GameObject prefabs")]
    [SerializeField] private GameObject prefabCirno;
    [SerializeField] private GameObject prefabWaka;
    [SerializeField] private GameObject prefabIceCream;

    [SerializeField] private GameObject prefabIsleTile;
    [SerializeField] private GameObject prefabIceTile;
    [SerializeField] private GameObject prefabRockTile;

    [SerializeField] private GameObject prefabLily;

    // Map related
    private TextAsset[]     textMap = new TextAsset[10];
    public static string[,] tile    = new string[10, 10];

    // Waka related (max 3 Waka per level)
    public static Vector2[] wakaStart = new Vector2[9];
    public static Vector2[] wakaEnd = new Vector2[9];
    public static Vector2[] wakaPass = new Vector2[9];

    public static GameObject[] wakaObject = new GameObject[9];

    [Header("Map Data")]
    public IcePath_MapData mapData;

    private Vector2 _origin;
    private float   _tileSize;
    private int     _mapAmount;
    private int     _mapWidth, _mapHeight;

    void Start() {
        // Assign privates
        _origin = mapData.origin;
        _tileSize = mapData.tileSize;

        _mapAmount  = mapData.mapAmount;
        _mapWidth   = mapData.mapWidth;
        _mapHeight  = mapData.mapHeight;

        textMap[0] = mapData.no1;
        textMap[1] = mapData.no2;
        textMap[2] = mapData.no3;
        textMap[3] = mapData.no4;
        textMap[4] = mapData.no5;
        textMap[5] = mapData.no6;
        textMap[6] = mapData.no7;
        textMap[7] = mapData.no8;
        textMap[8] = mapData.no9;
        textMap[9] = mapData.no10;

        // Initiate all tiles
        tile[_mapWidth, _mapHeight] = ".";

        // Read the map file
        int mapIndex = rand(0, _mapAmount - 1);

        if (alwaysLoadMapNo != 0) {
            mapIndex = alwaysLoadMapNo - 1;
        }

        string map = textMap[mapIndex].text;

        // Waka tiles indexer
        int wakaIndex = 0;

        // Generate the map

        for (int yy = 0; yy < _mapHeight; yy++) {
            for (int xx = 0; xx < _mapWidth; xx++) {

                // Determine spawn position
                string readPos = map.Substring(_mapWidth * yy + xx + 2 * yy, 1); // Add 2 yy for new line keyword in string

                Vector2 spawnPos = mapPos(xx, -yy);

                // Which tile to spawn?
                Quaternion quad = Quaternion.identity;

                GameObject waka;
                IcePath_Waka wakaScript;

                GameObject spawnObj;
                float randSize;

                switch (readPos) {
                    case "A": // Start isle tile
                        Instantiate(prefabIsleTile, spawnPos, quad);

                        GameObject cirno = Instantiate(prefabCirno, spawnPos, quad);
                        IcePath_Cirno cirnoScript = cirno.GetComponent<IcePath_Cirno>();

                        cirnoScript.cirnoGridX = xx;
                        cirnoScript.cirnoGridY = yy;
                        break;

                    case "B": // Finish isle tile
                        Instantiate(prefabIsleTile, spawnPos, quad);
                        Instantiate(prefabIceCream, spawnPos, quad);
                        break;

                    case "#": // Ice tile
                        Instantiate(prefabIceTile, spawnPos, quad);
                        break;

                    case "@": // Isle tile
                        Instantiate(prefabIsleTile, spawnPos, quad);
                        break;

                    // Wakas
                    case ">": // Waka passing - LEFT to RIGHT
                        Instantiate(prefabIceTile, spawnPos, quad);
                        Instantiate(prefabRockTile, mapPos(xx - 1, -yy), quad);
                        Instantiate(prefabRockTile, mapPos(xx + 1, -yy), quad);

                        wakaStart[wakaIndex] = mapPos(xx + 1, -yy);
                        wakaPass[wakaIndex] = mapPos(xx, -yy);
                        wakaEnd[wakaIndex] = mapPos(xx - 1, -yy);

                        waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                        wakaObject[wakaIndex] = waka;

                        waka.transform.Find("Rig").transform.localScale = new Vector3(-1, 1, 1);

                        wakaScript = waka.GetComponent<IcePath_Waka>();
                        wakaScript._wakaIndex = wakaIndex;
                        break;

                    case "<": // Waka passing - RIGHT to LEFT
                        Instantiate(prefabIceTile, spawnPos, quad);
                        Instantiate(prefabRockTile, mapPos(xx - 1, -yy), quad);
                        Instantiate(prefabRockTile, mapPos(xx + 1, -yy), quad);

                        wakaStart[wakaIndex] = mapPos(xx - 1, -yy);
                        wakaPass[wakaIndex] = mapPos(xx, -yy);
                        wakaEnd[wakaIndex] = mapPos(xx + 1, -yy);

                        waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                        wakaObject[wakaIndex] = waka;

                        wakaScript = waka.GetComponent<IcePath_Waka>();
                        wakaScript._wakaIndex = wakaIndex;
                        break;

                    case "^": // Waka passing - BOTTOM to TOP
                        Instantiate(prefabIceTile, spawnPos, quad);
                        Instantiate(prefabRockTile, mapPos(xx, -(yy - 1)), quad);
                        Instantiate(prefabRockTile, mapPos(xx, -(yy + 1)), quad);

                        wakaStart[wakaIndex] = mapPos(xx, -(yy - 1));
                        wakaPass[wakaIndex] = mapPos(xx, -yy);
                        wakaEnd[wakaIndex] = mapPos(xx, -(yy + 1));

                        waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                        wakaObject[wakaIndex] = waka;

                        wakaScript = waka.GetComponent<IcePath_Waka>();
                        wakaScript._wakaIndex = wakaIndex;
                        break;

                    case "v": // Waka passing - TOP to BOTTOM
                        Instantiate(prefabIceTile, spawnPos, quad);
                        Instantiate(prefabRockTile, mapPos(xx, -(yy - 1)), quad);
                        Instantiate(prefabRockTile, mapPos(xx, -(yy + 1)), quad);

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
                        Instantiate(prefabLily, spawnPos, quad);
                        break;

                    case ".": // Nothing
                        break;

                    default:
                        break;
                }
                // Write this down in the grid
                if (readPos == "<" ||
                    readPos == ">" ||
                    readPos == "^" ||
                    readPos == "v") {
                    // Write down Waka index if it is one
                    tile[xx, yy] = wakaIndex.ToString();
                    wakaIndex++;
                }
                else {
                    tile[xx, yy] = readPos;
                }

            }
        }

    }

    Vector2 mapPos(float posX, float posY) {
        return (_origin + _tileSize * new Vector2(posX, posY));
    }

    int rand(float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
