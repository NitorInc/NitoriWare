using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_script_GenerateMap : MonoBehaviour {

    [Header("GameObject prefabs")]
    [SerializeField] private GameObject camera;

    [SerializeField] private GameObject prefabCirno;
    [SerializeField] private GameObject prefabWaka;

    [SerializeField] private GameObject prefabIceCream;
    [SerializeField] private GameObject prefabLamp;

    [SerializeField] private GameObject prefabIsleTile;
    [SerializeField] private GameObject prefabIceTile;

    [SerializeField] private GameObject prefabLily1;
    [SerializeField] private GameObject prefabLily2;
    [SerializeField] private GameObject prefabLily3;

    [SerializeField] private GameObject prefabCattail1;
    [SerializeField] private GameObject prefabCattail2;

    [SerializeField] private GameObject prefabFog;

    // Map related
    private TextAsset[] textMap = new TextAsset[5];

    public static string[,] tile = new string[10, 10];

    // Waka related (max 3 Waka per level)
    public static Vector2[] wakaStart = new Vector2[3];
    public static Vector2[] wakaEnd = new Vector2[3];
    public static Vector2[] wakaPass = new Vector2[3];

    public static GameObject[] wakaObject = new GameObject[2];

    [Header("Map Data")]
    public IcePath_script_MapData mapData;

    private int _mapWidth, _mapHeight;
    private Vector2 _origin;

    void Start() {
        // Assign privates
        _mapWidth = mapData.mapWidth;
        _mapHeight = mapData.mapHeight;
        _origin = mapData.origin;

        textMap[0] = mapData.a;
        textMap[1] = mapData.b;
        textMap[2] = mapData.c;
        textMap[3] = mapData.d;
        textMap[4] = mapData.e;

        // Initiate all tiles
        tile[_mapWidth, _mapHeight] = ".";

        // Read the map file
        int mapIndex = rand(0, 2);
        string map = textMap[mapIndex].text;

        // Waka tiles indexer
        int wakaIndex = 0;

        // Generate the map!
        for (int i = 0; i < map.Length; i++) {

            // Determine spawn position
            string readPos = map.Substring(i, 1);

            int tilePosX = i % _mapWidth;
            int tilePosY = Mathf.FloorToInt(i / _mapWidth);

            Vector2 spawnPos = _origin + new Vector2(tilePosX, -tilePosY);

            // Which tile to spawn?
            Quaternion quad = Quaternion.identity;
            GameObject waka;
            IcePath_script_Waka wakaScript;

            switch (readPos) {
                case "A": // Start isle tile
                    Instantiate(prefabIsleTile, spawnPos, quad);

                    GameObject cirno = Instantiate(prefabCirno, spawnPos, quad);
                    IcePath_script_Cirno cirnoScript = cirno.GetComponent<IcePath_script_Cirno>();

                    cirnoScript.cirnoGridX = tilePosX;
                    cirnoScript.cirnoGridY = tilePosY;
                    break;

                case "B": // Finish isle tile
                    Instantiate(prefabIsleTile, spawnPos, quad);
                    Instantiate(prefabIceCream, spawnPos, quad);

                    camera.transform.position = spawnPos;
                    break;

                case "#": // Ice tile
                    Instantiate(prefabIceTile, spawnPos, quad);
                    break;

                case "O": // Lamp
                    Instantiate(prefabLamp, spawnPos, quad);
                    break;

                // Wakas
                case ">": // Waka passing - LEFT to RIGHT
                    Instantiate(prefabIceTile, spawnPos, quad);

                    wakaStart[wakaIndex] = _origin + new Vector2(tilePosX - 1, -tilePosY);
                    wakaPass[wakaIndex] = _origin + new Vector2(tilePosX, -tilePosY);
                    wakaEnd[wakaIndex] = _origin + new Vector2(tilePosX + 1, -tilePosY);

                    waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                    wakaScript = waka.GetComponent<IcePath_script_Waka>();

                    wakaObject[wakaIndex] = waka;
                    wakaScript._wakaIndex = wakaIndex;
                    break;

                case "<": // Waka passing - RIGHT to LEFT
                    Instantiate(prefabIceTile, spawnPos, quad);

                    wakaStart[wakaIndex] = _origin + new Vector2(tilePosX + 1, -tilePosY);
                    wakaPass[wakaIndex] = _origin + new Vector2(tilePosX, -tilePosY);
                    wakaEnd[wakaIndex] = _origin + new Vector2(tilePosX - 1, -tilePosY);

                    waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                    wakaScript = waka.GetComponent<IcePath_script_Waka>();

                    wakaObject[wakaIndex] = waka;
                    wakaScript._wakaIndex = wakaIndex;
                    break;

                case "^": // Waka passing - BOTTOM to TOP
                    Instantiate(prefabIceTile, spawnPos, quad);

                    wakaStart[wakaIndex] = _origin + new Vector2(tilePosX, -(tilePosY - 1));
                    wakaPass[wakaIndex] = _origin + new Vector2(tilePosX, -tilePosY);
                    wakaEnd[wakaIndex] = _origin + new Vector2(tilePosX, -(tilePosY + 1));

                    waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                    wakaScript = waka.GetComponent<IcePath_script_Waka>();

                    wakaObject[wakaIndex] = waka;
                    wakaScript._wakaIndex = wakaIndex;
                    break;

                case "v": // Waka passing - TOP to BOTTOM
                    Instantiate(prefabIceTile, spawnPos, quad);

                    wakaStart[wakaIndex] = _origin + new Vector2(tilePosX, -(tilePosY + 1));
                    wakaPass[wakaIndex] = _origin + new Vector2(tilePosX, -tilePosY);
                    wakaEnd[wakaIndex] = _origin + new Vector2(tilePosX, -(tilePosY - 1));

                    waka = Instantiate(prefabWaka, wakaStart[wakaIndex], quad);
                    wakaScript = waka.GetComponent<IcePath_script_Waka>();

                    wakaObject[wakaIndex] = waka;
                    wakaScript._wakaIndex = wakaIndex;
                    break;

                // Lillies
                case "q": // Lily 1
                    Instantiate(prefabLily1, spawnPos, quad);
                    break;
                case "w": // Lily 2
                    Instantiate(prefabLily2, spawnPos, quad);
                    break;
                case "e": // Lily 3
                    Instantiate(prefabLily3, spawnPos, quad);
                    break;

                // Cattails
                case "r": // Cattail 1
                    Instantiate(prefabCattail1, spawnPos, quad);
                    break;
                case "t": // Cattail 2
                    Instantiate(prefabCattail2, spawnPos, quad);
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
                tile[tilePosX, tilePosY] = wakaIndex.ToString();
                wakaIndex++;
            }
            else {
                tile[tilePosX, tilePosY] = readPos;
            }

        }

    }

    int rand(float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
