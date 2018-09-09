using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_script_Cirno : MonoBehaviour {

    private bool isHit = false;
    private bool isWon = false;
    
    [Header("Hit audio")]
    public AudioClip hitSound;

    Animator anim;

    // Cirno's position
    [HideInInspector]   public int      cirnoGridX, cirnoGridY; // Current  position in grid array
                        private Vector2 cirnoEndPos;            // Next     position in scene 

    // Get the grid map
    private string[,] _tile = IcePath_script_GenerateMap.tile;

    [Header("Map data file")]
    public IcePath_script_MapData mapData;

    private int     _mapWidth, _mapHeight;
    private Vector2 _origin;

    void Start () {
        // Assign privates
        _mapWidth    = mapData.mapWidth;
        _mapHeight   = mapData.mapHeight;
        _origin      = mapData.origin;

        _tile        = IcePath_script_GenerateMap.tile;

        // Set starting position
        transform.position = _origin + new Vector2(cirnoGridX, -cirnoGridY);
        cirnoEndPos = transform.position;

        // Animation
        anim = GetComponentInChildren<Animator>();
	}
	
	void Update () {
        // Is this the ice cream?
        if (_tile[cirnoGridX, cirnoGridY] == "B" &&
            transform.position == (Vector3)cirnoEndPos) {
            if (!isWon) {
                Win();
                isWon = true;

                MicrogameController.instance.setVictory(victory: true, final: true);

            }
        }

        // Is this a Waka passing?
        int wakaIndex;

        if (int.TryParse(_tile[cirnoGridX, cirnoGridY], out wakaIndex)) {
            // Is Waka passing through?
            GameObject          waka        = IcePath_script_GenerateMap.wakaObject[wakaIndex];
            IcePath_script_Waka wakaScript  = waka.GetComponent<IcePath_script_Waka>();

            if (!wakaScript.isPassable) {
                if (!isHit) {
                    // Get hit
                    Die();
                    isHit = true;

                    MicrogameController.instance.playSFX(hitSound, volume: 0.75f,
                        panStereo: AudioHelper.getAudioPan(transform.position.x));

                    MicrogameController.instance.setVictory(victory: false, final: true);

                }
            }

        }

        // Has Cirno been hit?
        if (isHit) {
            // Lose condition - fly away now
            transform.position = transform.position + (new Vector3(-8, 8, 0) * Time.deltaTime);
            transform.Rotate(new Vector3(0, 0, 270 * Time.deltaTime));

        } else

        // Has Cirno won?
        if (isWon) {

            /* oh. she has? alright */

        } else
        
        // Move on as usual
        {

            // Is Cirno locked into her current grid yet?
            if (((Vector2)transform.position - cirnoEndPos).magnitude > 0.33f) {
                transform.position = Vector2.Lerp(transform.position, cirnoEndPos, 0.5f);

            } else
            // Lock her into place and check for the next movement
            {
                transform.position = cirnoEndPos;

                // Movement
                int moveX = (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0)  - (Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0);
                int moveY = (Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0)     - (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0);

                // Player is moving
                if (moveX != 0 ||
                    moveY != 0) {
                    // Valid movement?
                    if (canWalkInto(cirnoGridX + moveX, cirnoGridY - moveY)) {
                        cirnoGridX += moveX;
                        cirnoGridY -= moveY;

                        cirnoEndPos = _origin + new Vector2(cirnoGridX, -cirnoGridY);
                    }
                }

            }

        }

    }

    void Die() {
        // Explosion I guess
        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        particle.Play();

    }

    void Win() {
        // Animation I guess
        anim.SetBool("TriggerVictory", true);

        // Zoom in on me!
        GameObject camera = GameObject.Find("CameraMover");
        IcePath_script_Camera cameraScript = camera.GetComponent<IcePath_script_Camera>();

        cameraScript.zoomState = 2;

    }

    bool canWalkInto(int posX, int posY) {
        // Can Cirno walk here?

        if (isWithin(posX, 0, _mapWidth - 1) && // Is the position within the grid array?
            isWithin(posY, 0, _mapHeight - 1)) {
            
            return (_tile[posX, posY] == "A" || // Is it the start isle?
                    _tile[posX, posY] == "B" || // Is it the end isle?
                    _tile[posX, posY] == "#" || // Is it an ice tile?
                    _tile[posX, posY] == "0" || // Is it a Waka passing?
                    _tile[posX, posY] == "1" ||
                    _tile[posX, posY] == "2");

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
