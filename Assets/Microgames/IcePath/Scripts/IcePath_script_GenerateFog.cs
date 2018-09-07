using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_script_GenerateFog : MonoBehaviour {

    [Header("Fog-related")]
    public GameObject   prefabFog;
    public float        fogTime;

    float fogAlarm;

    [Header("Map Data")]
    public IcePath_script_MapData mapData;

    private int _mapDiff;
    private Vector2 _origin;
    
    void Start() {
        // Assign privates
        _mapDiff = mapData.mapDiff;
        _origin  = mapData.origin;

        // Spawn a few fog at first
        if (_mapDiff == 3) {

            int n = 32;
            while (n > 0) {
                Instantiate(prefabFog, new Vector2(rand(-16, 16), rand(-12, 12)), Quaternion.identity);
                n--;
            }
        }
	}
	
	void Update () {
        
    }

    int rand (float min, float max) {
        // Return: round random range value
        return (Mathf.RoundToInt(Random.Range(min, max)));
    }
}
