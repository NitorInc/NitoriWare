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

    [Header("Map files")]
    public TextAsset a;
    public TextAsset b;
    public TextAsset c;
    public TextAsset d;
    public TextAsset e;
    public TextAsset f;
    public TextAsset g;
    public TextAsset h;
    public TextAsset i;
    public TextAsset j;

    void Awake() {
        // Set the origin
        origin = new Vector2(-(mapWidth - 1)/ 2, (mapHeight - 1)/ 2);
    }

}
