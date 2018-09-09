using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_script_Waka : MonoBehaviour {

    // My index
    [HideInInspector] public int _wakaIndex;
    
    [Header("Leap time")]
    [SerializeField] private float leapTime;
    private float leapAlarm;

    // Tile info
    private Vector2[]   tilePos = new Vector2[2];
    public int         tileCurrent = 0;

    private int start = 0;
    private int finish = 1;

    [HideInInspector] public bool isPassable = true;
    
	void Start () {
        // The tiles
        tilePos[0] = IcePath_script_GenerateMap.wakaStart[_wakaIndex];
        tilePos[1] = IcePath_script_GenerateMap.wakaEnd[_wakaIndex];

        transform.position = tilePos[0];

    }
	
	void Update () {

        Vector2 startTile    = tilePos[start];
        Vector2 finishTile   = tilePos[finish];

        // Leap
        transform.position = Vector2.Lerp(transform.position, finishTile, 0.2f);

        // Set if passable
        bool isMoreThanMin = ((Vector2)transform.position - finishTile).magnitude > 0.33f;
        bool isLessThanMax = ((Vector2)transform.position - finishTile).magnitude < 0.66f;

        isPassable = !(isMoreThanMin && isLessThanMax);

        // Leap timing
        if (leapAlarm > 0) {
            leapAlarm -= Time.deltaTime;
        } else {

            tileCurrent = tileCurrent == 0 ? 1 : 0;

            start   = tileCurrent;
            finish  = tileCurrent == 0 ? 1 : 0;

            leapAlarm = leapTime;

        }

	}

    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }

}
