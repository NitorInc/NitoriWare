using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Cirno : MonoBehaviour {
    // Cirno's position
    public static int   cirnoGridX, cirnoGridY;   // Coordinate in grid array

    Vector2 cirnoGoalPos;   // Goal position in scene

    // Cirno's sway
    int cirnoSway;

    bool isHit = false;

    // Bring in static variables
    int mapWidth;
    int mapHeight;
    float tileSize;
    Vector2 origin;

    string[,] tile  = IcePath_Generate.globalTile;


    void Start () {
        // Bring in static variables
        mapWidth    = IcePath_Master.globalMapWidth;
        mapHeight   = IcePath_Master.globalMapHeight;
        tileSize    = IcePath_Master.globalTileSize;
        origin      = IcePath_Master.globalOrigin;

        tile        = IcePath_Generate.globalTile;

        // Set starting position
        cirnoGridX = IcePath_Generate.cirnoStartX;
        cirnoGridY = IcePath_Generate.cirnoStartY;

        transform.position = origin + tileSize * new Vector2(cirnoGridX, -cirnoGridY);
        cirnoGoalPos = transform.position;

        cirnoSway = 1;
	}
	
	void Update () {
        // Did Waka hit Cirno?
        if (tile[cirnoGridX, cirnoGridY] == "b" &&
            !IcePath_Waka.isPassable) {

            if (!isHit) {
                Die();
                isHit = true;
            }
        }

        if (isHit) {
            // Fly away now
            transform.position = transform.position + (new Vector3(-15, 15, 0) * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));

        } else {

            // Is Cirno locked into her current grid yet?
            if (((Vector2)transform.position - cirnoGoalPos).magnitude > 0.33f) {
                transform.position = Vector2.Lerp(transform.position, cirnoGoalPos, 0.5f);

            } else
            // Lock her into place and check for the next movement
            {
                transform.position = cirnoGoalPos;

                // Movement
                int moveX = (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0);
                int moveY = (Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0);

                // Player is moving
                if (moveX != 0 ||
                    moveY != 0) {

                    // Valid movement?
                    if (canWalkInto(cirnoGridX + moveX, cirnoGridY - moveY)) {
                        cirnoGridX += moveX;
                        cirnoGridY -= moveY;

                        cirnoGoalPos = origin + tileSize * new Vector2(cirnoGridX, -cirnoGridY);

                        // Make Cirno sway side-to-side for every step
                        cirnoSway *= -1;
                    }
                }
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 8f * cirnoSway));

            }

        }

    }

    void Die() {
        // Explosion I guess
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        particle.Play();

    }

    bool canWalkInto(int posX, int posY) {
        // Can Cirno walk here?

        if (isWithin(posX, 0, mapWidth - 1) && // Is the position within the grid array?
            isWithin(posY, 0, mapHeight - 1)) {
            
            return (tile[posX, posY] == "A" || // Is it the start isle?
                    tile[posX, posY] == "B" || // Is it the end isle?
                    tile[posX, posY] == "b" || // Is it Wakasagihime's passing tile?
                    tile[posX, posY] == "#");  // Is it an ice tile?

        } else {
            return false;
        }

    }

    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }

}
