using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePath_Waka : MonoBehaviour {

    Animator animator;

    [Header("Sprites")]
    [SerializeField] Sprite spriteIdle;
    [SerializeField] Sprite spriteLeap;
    [SerializeField] AudioClip splashClip;

    // My index
    [HideInInspector] public int _wakaIndex;

    static IcePath_Waka splashInstance;

    // Leap related
    private Vector3 wakaSpeed = Vector3.zero;

    [Header("Leap time")]
    [SerializeField] private float leapTime;
    [SerializeField] private float leapDuration;
    private float leapAlarm;

    private bool isLeaping = true;
    [HideInInspector] public bool isPassable = true;

    // Tile info
    private Vector2[] tilePos = new Vector2[2];
    private int tileCurrent = 0;

    private int start = 0;
    private int finish = 1;

    private void Awake()
    {
        splashInstance = null;
    }

    void Start () {
        // The tiles
        tilePos[0] = IcePath_GenerateMap.wakaStart[_wakaIndex];
        tilePos[1] = IcePath_GenerateMap.wakaEnd[_wakaIndex];

        transform.position = tilePos[1];

        // Animator
        animator = transform.Find("Rig").Find("Animation").GetComponent<Animator>();

        leapAlarm = leapTime;

        if (splashInstance == null)
            splashInstance = this;

    }
	
	void Update () {

        Vector2 startTile    = tilePos[start];
        Vector2 finishTile   = tilePos[finish];

        // Leap
        if (((Vector2)transform.position - finishTile).magnitude > .025f)
            transform.position = Vector3.SmoothDamp(transform.position, finishTile, ref wakaSpeed, leapDuration, Mathf.Infinity, Time.deltaTime);
        //else
        //    transform.position = finishTile;

        // Set if passable
        Vector2 center = startTile + (finishTile - startTile) / 2;
        bool isFarFromCenter = ((Vector2)transform.position - center).magnitude > 0.25f;

        isPassable = isFarFromCenter;

        // Leap timing
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("IcePath_WakaAnim")) {
            animator.SetBool("hasLeaped", false);
        }
        
        if (leapAlarm > 0) {

            leapAlarm -= Time.deltaTime;

        } else {

            tileCurrent = tileCurrent == 0 ? 1 : 0;

            start   = tileCurrent;
            finish  = tileCurrent == 0 ? 1 : 0;

            animator.SetBool("hasLeaped", true);

            Transform rig = transform.Find("Rig").transform;
            rig.localScale = new Vector3(rig.localScale.x * -1, 1, 1);

            leapAlarm = leapTime;

            if (splashInstance == this && MicrogameTimer.instance.beatsLeft <= 15f)
            {
                MicrogameController.instance.playSFX(splashClip,
                    volume: .35f);
            }

        }

    }


    bool isWithin(float input, float min, float max) {
        // Is the value within this range?
        return (input >= min &&
                input <= max);
    }

}
