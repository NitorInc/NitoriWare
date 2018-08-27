using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Cirno : MonoBehaviour {
    // Cirno's position
    public static int cirnoX, cirnoY;

	// Use this for initialization
	void Start () {
        // Set starting position
        transform.position = IcePath_Generate.tileSize * new Vector2(cirnoX, cirnoY) + IcePath_Generate.origin;
		
	}
	
	// Update is called once per frame
	void Update () {
        // Movement
        int moveX = (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0);
        int moveY = (Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0);

        if (canWalkInto(cirnoX + moveX, cirnoY - moveY)) {
            cirnoX += moveX;
            cirnoY -= moveY;

            transform.position = IcePath_Generate.tileSize * new Vector2(cirnoX, -cirnoY) + IcePath_Generate.origin;
        }
    }

    bool canWalkInto(int posX, int posY) {
        // Can Cirno walk here?
        return (isWithin(posX, 0, IcePath_Generate.globalMapWidth - 1)  && // Is the position within the grid array?
                isWithin(posY, 0, IcePath_Generate.globalMapHeight - 1) &&
                IcePath_Generate.tile[posX, posY] != "." && // Is the tile not empty?
                IcePath_Generate.tile[posX, posY] != "O" && // Is the tile not a lamp?
                IcePath_Generate.tile[posX, posY] != "W" && // Is the tile not Wakasagihime?
                IcePath_Generate.tile[posX, posY] != "@");  // You get the point.
    }

    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }
}
