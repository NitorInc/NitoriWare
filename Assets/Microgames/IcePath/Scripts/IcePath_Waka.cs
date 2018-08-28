using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Waka : MonoBehaviour {

    [Header("Leap time")]
    [SerializeField]
    private float leapTime;
    float leapAlarm;

    [Header("Air time")]
    [SerializeField]
    private float airTime;
    float airAlarm;

    // Tile info
    Vector2[] tilePos = new Vector2[2];
    int tileCurrent;

    public static bool isPassable = true;

	// Use this for initialization
	void Start () {
        // The tiles
        tilePos[0] = IcePath_Generate.wakaStart;
        tilePos[1] = IcePath_Generate.wakaEnd;

        tileCurrent = 0;

        airAlarm = airTime;
    }
	
	// Update is called once per frame
	void Update () {
        // Leap timing

        if (leapAlarm > 0) {
            leapAlarm -= Time.deltaTime;
        } else {

            int start = tileCurrent;
            int finish = tileCurrent == 0 ? 1 : 0;

            Vector2 startTile = tilePos[start];
            Vector2 finishTile = tilePos[finish];

            Vector2 direction = (finishTile - startTile).normalized;
            float dist = (finishTile - startTile).magnitude;
            float speed = dist / airTime;

            // Leap into the air
            if (airAlarm > 0) {
                transform.position = (Vector2)transform.position + (direction * speed * Time.deltaTime);

                // Set if passable
                isPassable = isWithin(airAlarm, 0.2f * airTime, 0.8f * airTime);

                airAlarm -= Time.deltaTime;
            } else {
                transform.position = finishTile;

                // Set if passable
                isPassable = true;

                tileCurrent = finish;

                leapAlarm = leapTime;
                airAlarm = airTime;
            }

        }

	}

    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }

}
