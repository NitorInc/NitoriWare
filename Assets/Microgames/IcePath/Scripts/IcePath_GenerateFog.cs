using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class IcePath_GenerateFog : MonoBehaviour {

    // Fog-related variables
    [Header("Fog-related")]
    [SerializeField] private GameObject prefabFog;
    [SerializeField] private float      fogTime;
    float                               fogAlarm;

    // Bring in static variables
    int mapDiff;
    Vector2 origin;
    
    void Awake() {
        // Assign static variables
        mapDiff = IcePath_Master.globalMapDiff;
        origin  = IcePath_Master.globalOrigin;

    }

	void Start () {
        // Spawn a few fog at first
        if (mapDiff == 3) {

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
