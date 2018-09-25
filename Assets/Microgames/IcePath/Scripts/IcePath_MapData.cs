using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MapData", fileName = "New MapData")]
public class IcePath_MapData : ScriptableObject {

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

}
