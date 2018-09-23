using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MapData", fileName = "New MapData")]
public class IcePath_MapData : ScriptableObject {

    [Header("Tile size")]
    public float tileSize;

    [Header("Map difficulty")]
    public int mapDiff;

    [Header("Map size")]
    public int mapWidth;
    public int mapHeight;

    [HideInInspector] public Vector2 origin;

    [Header("Number of maps available")]
    public int mapAmount;

    [Header("Map files")]
    public TextAsset no1;
    public TextAsset no2;
    public TextAsset no3;
    public TextAsset no4;
    public TextAsset no5;
    public TextAsset no6;
    public TextAsset no7;
    public TextAsset no8;
    public TextAsset no9;
    public TextAsset no10;

    void Awake() {
        // Set the origin
        origin = new Vector2(-(mapWidth - 1)/ 2, (mapHeight - 1)/ 2);

    }

}
