using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Cirno : MonoBehaviour {
    // Cirno's position
    public static int   cirnoX, cirnoY;           // Coordinate in grid array
    public static float cirnoTrueX, cirnoTrueY;   // Coordinate in scene

    // Cirno's sway
    int cirnoSway;

    bool isHit = false;

    // Use this for initialization
    void Start () {
        // Set starting position
        transform.position = IcePath_Generate.tileSize * new Vector2(cirnoX, cirnoY) + IcePath_Generate.origin;

        cirnoSway = 1;
	}
	
	// Update is called once per frame
	void Update () {

        // Hit Waka?

        if (IcePath_Generate.tile[cirnoX, cirnoY] == "b") {
            if (!IcePath_Waka.isPassable) {

                if (!isHit) {
                    Die();
                    isHit = true;
                }
                
            }
        }

        if (isHit) {
            // Fly away now
            transform.position = transform.position + (new Vector3(-15, 15, 0) * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, 90 * Time.deltaTime));

        } else {

            // Movement
            int moveX = (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0);
            int moveY = (Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0);

            // Player is moving
            if (moveX != 0 ||
                moveY != 0) {

                // Valid movement?
                if (canWalkInto(cirnoX + moveX, cirnoY - moveY)) {
                    cirnoX += moveX;
                    cirnoY -= moveY;

                    transform.position = IcePath_Generate.tileSize * new Vector2(cirnoX, -cirnoY) + IcePath_Generate.origin;

                    cirnoTrueX = transform.position.x;
                    cirnoTrueY = transform.position.y;

                    // Make Cirno sway side-to-side for every step
                    cirnoSway *= -1;
                }
            }
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 8f * cirnoSway));

        }

    }

    void Die() {
        // Explosion I guess
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        particle.Play();

    }

    bool canWalkInto(int posX, int posY) {
        // Can Cirno walk here?

        if (isWithin(posX, 0, IcePath_Generate.globalMapWidth - 1) && // Is the position within the grid array?
            isWithin(posY, 0, IcePath_Generate.globalMapHeight - 1)) {

            string tile = IcePath_Generate.tile[posX, posY];

            return (tile == "A" || // Is it the start isle?
                    tile == "B" || // Is it the end isle?
                    tile == "b" || // Is it Wakasagihime's passing tile?
                    tile == "#");  // Is it an ice tile?

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
