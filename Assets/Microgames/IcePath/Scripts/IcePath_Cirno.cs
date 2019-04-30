using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Cirno : MonoBehaviour {

    [HideInInspector] public GameObject icecream;
    [SerializeField] private int diff;
    [SerializeField] private AudioSource sfxSource;

    private bool isHit = false;
    private bool hasWon = false;

    Animator anim;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip moveClip;
    public AudioClip victoryClip;

    [Header("Tilt settings")]
    [SerializeField] private float  tiltAngle = 10f;
    [SerializeField] private float  tiltSpeed = 100f;
    [SerializeField] private float  moveSpeed = 30f;
    [SerializeField] private        Transform tiltPivot;

    private float tiltDirection;
    private float currentAngle;
    float getTiltAngleGoal() => tiltAngle * tiltDirection;

    // Cirno's position
    [HideInInspector]   public int      cirnoGridX, cirnoGridY; // Current  position in grid array
                        private Vector2 cirnoEndPos;            // Next     position in scene
    private float cirnoSpeed = 0f;

    // Get the grid map
    private string[,] _tile;

    private Vector2 origin;

    void Start () {

        origin = new Vector2(-5.5f, 4);

        // Assign privates
        _tile        = IcePath_GenerateMap.tile;

        // Set starting position
        transform.position = mapPos(cirnoGridX, -cirnoGridY);
        cirnoEndPos = transform.position;

        // Animation
        anim = GetComponentInChildren<Animator>();
        tiltPivot.eulerAngles = Vector3.forward * getTiltAngleGoal();
	}
	
	void Update () {

        // Is this the ice cream?
        if (_tile[cirnoGridX, cirnoGridY] == "B" &&
            MathHelper.Approximately(transform.position.x, cirnoEndPos.x, .01f) &&
            MathHelper.Approximately(transform.position.y, cirnoEndPos.y, .01f)) {

            if (!hasWon) {
                Win();
                hasWon = true;
                tiltDirection = 0f;
            }

        }

        // Update angle
        currentAngle = Mathf.MoveTowards(currentAngle, getTiltAngleGoal(), tiltSpeed * Time.deltaTime);
        tiltPivot.eulerAngles = Vector3.forward * currentAngle;

        // Is this a Waka crossing?
        int wakaIndex;

        if (int.TryParse(_tile[cirnoGridX, cirnoGridY], out wakaIndex)) {
            // Is Waka crossing through?
            GameObject   waka        = IcePath_GenerateMap.wakaObject[wakaIndex];
            IcePath_Waka wakaScript  = waka.GetComponent<IcePath_Waka>();

            if (!wakaScript.isPassable) {
                if (!isHit) {
                    // Get hit

                    Die();
                    isHit = true;

                    sfxSource.PlayOneShot(hitSound);

                    MicrogameController.instance.setVictory(victory: false, final: true);

                }
            }

        }

        // Has Cirno been hit?
        if (isHit) {
            // Lose state - fly away now
            transform.position = transform.position + (new Vector3(-8, 8, 0) * Time.deltaTime);
            transform.Find("Spin Pivot").Find("Rig").Rotate(new Vector3(0, 0, 270 * Time.deltaTime));

        } else

        // Has Cirno won?
        if (hasWon) {

            /* oh. she has? alright */

        } else
        
        // Move on as usual
        {

            // Move Cirno towards grid if applicable
            MathHelper.moveTowards2D(transform, cirnoEndPos, moveSpeed);
            
            // Movement
            int moveX = (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0)  - (Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0);
            int moveY = (Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0)     - (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0);

            if (moveX != 0) {
                moveY = 0;
            } else {
                moveX = 0;
            }

            // Player is moving
            if (moveX != 0 ||
                moveY != 0) {
                // Valid movement?
                if (canWalkInto(cirnoGridX + moveX, cirnoGridY - moveY)) {
                    cirnoGridX += moveX;
                    cirnoGridY -= moveY;

                    transform.position = cirnoEndPos;   //Snap to next block
                    cirnoEndPos = mapPos(cirnoGridX, -cirnoGridY);

                    MicrogameController.instance.playSFX(moveClip,
                        pitchMult: Random.Range(.96f, 1.04f),
                        panStereo: AudioHelper.getAudioPan(transform.position.x),
                        volume: .5f);
                    tiltDirection = tiltDirection == 0f ? -1f : -tiltDirection;
                    if (moveX != 0 && (float)moveX != Mathf.Sign(tiltPivot.localScale.x))
                        tiltPivot.localScale = new Vector3(-tiltPivot.localScale.x, tiltPivot.localScale.y, tiltPivot.localScale.z);
                }
            }

        }

    }

    void Die() {
        // Explosion I guess
        Transform shadow = transform.Find("Shadow");
        Destroy(shadow.gameObject);

        ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
        particle.Play();

    }

    void Win() {
        // Animation I guess
        MicrogameController.instance.setVictory(victory: true, final: true);
        MicrogameController.instance.playSFX(victoryClip);

        anim.SetBool("hasWon", true);

    }

    bool canWalkInto(int posX, int posY) {
        // Can Cirno walk here?

        if (isWithin(posX, 0, 11) && // Is the position within the grid array?
            isWithin(posY, 0, 8)) {
            
            return (_tile[posX, posY] == "A" || // Is it the start isle?
                    _tile[posX, posY] == "B" || // Is it the end isle?
                    _tile[posX, posY] == "#" || // Is it an ice tile?
                    _tile[posX, posY] == "@" || // Is it an isle tile?
                    _tile[posX, posY] == "0" || // Is it a Waka passing?
                    _tile[posX, posY] == "1" ||
                    _tile[posX, posY] == "2" ||
                    _tile[posX, posY] == "3" ||
                    _tile[posX, posY] == "4" ||
                    _tile[posX, posY] == "5" ||
                    _tile[posX, posY] == "6" ||
                    _tile[posX, posY] == "7" ||
                    _tile[posX, posY] == "8");

        } else {
            return false;
        }

    }

    Vector2 mapPos(float posX, float posY) {
        return (new Vector2(-5.5f + posX, 4 + posY));
    }

    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }

}
