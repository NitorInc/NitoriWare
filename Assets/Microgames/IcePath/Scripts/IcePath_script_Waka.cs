using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_script_Waka : MonoBehaviour {

    // My index
    [HideInInspector] public int _wakaIndex;
    
    [Header("Leap time")]
    [SerializeField]
    private float   leapTime;
    float           leapAlarm;
    
    [Header("Air time")]
    [SerializeField]
    private float   airTime;
    float           airAlarm;

    // Tile info
    Vector2[]   tilePos = new Vector2[2];
    int         tileCurrent;

    [HideInInspector] public bool isPassable = true;
    
	void Start () {
        // The tiles
        tilePos[0] = IcePath_script_GenerateMap.wakaStart[_wakaIndex];
        tilePos[1] = IcePath_script_GenerateMap.wakaEnd[_wakaIndex];

        tileCurrent = 0;

        airAlarm = airTime;
    }
	
	void Update () {

        // Leap timing

        if (leapAlarm > 0) {
            leapAlarm -= Time.deltaTime;
        } else {

            // Prepare the variables
            int start   = tileCurrent;
            int finish  = tileCurrent == 0 ? 1 : 0;

            Vector2 startTile   = tilePos[start];
            Vector2 finishTile  = tilePos[finish];

            Vector2 direction   = (finishTile - startTile).normalized;
            float dist          = (finishTile - startTile).magnitude;
            float speed         = dist / airTime;


            // Leap into the air

            if ((((Vector2)transform.position) - finishTile).magnitude > 0.33f) {
                transform.position = (Vector2)transform.position + (direction * speed * Time.deltaTime);

                isPassable = isWithin(airAlarm, 0.2f * airTime, 0.4f * airTime);
                airAlarm -= Time.deltaTime;

            } else {
                transform.position = finishTile;
                tileCurrent = finish;

                isPassable = true;
                airAlarm = airTime;

                leapAlarm = leapTime;

            }


        }

	}

    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }

}
