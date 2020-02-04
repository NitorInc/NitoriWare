using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks_WeakPoint
    : MonoBehaviour
{

    public static int weakpointCount = 0;

    [SerializeField]
    private bool flipped;

    [SerializeField]
    private GameObject yukari;

    [SerializeField]
    private GameObject rope;

    [SerializeField]
    private Sprite sadYukari;

    [SerializeField]
    private Sprite cutRope;

    [SerializeField]
    private float volleySpeed = 0f;

    [SerializeField]
    private AudioClip snapClip;

    int ropeDespawn = -1;
    float tValue;   // Value between 0 (min position) and 1 (max position) where the weak point is, moves in later difficulties
    bool goingRight;
    bool snapped;

    // Use this for initialization
    void Start() {
        //Place weak points on rope
        weakpointCount = transform.root.childCount;
        //DATA: Values selected to ensure randomization places weak point on rope
        tValue = Random.Range(0f, 1f);
        transform.localPosition = getLerpedPosition(tValue);
        goingRight = MathHelper.randomBool();
    }

    // t is a value between 0 and 1
    Vector3 getLerpedPosition(float t)
    {
        var startx = (t * 4f) - 2f; // Convert t to value between -2 and 2
        float yfactor = -0.21f;
        if (flipped)
        {
            yfactor *= -1;
        }
        return new Vector3(startx, (startx - 0.5f) * yfactor, 0);
    }

    // Update is called once per frame
    void Update() {
        if (volleySpeed > 0f && !snapped)
        {
            var frameDelta = volleySpeed * Time.deltaTime;
            var goalT = goingRight ? 1f : 0f;
            tValue = Mathf.MoveTowards(tValue, goalT, frameDelta);
            if (tValue == goalT)
                goingRight = !goingRight;
            transform.localPosition = getLerpedPosition(tValue);
        }
        if (ropeDespawn >= 0 && ropeDespawn <= 30)
        {
            ropeDespawn++;
            if (ropeDespawn == 10)
            {
                rope.SetActive(false);
            }
        }
    }

    void OnMouseDown() {
        snapped = true;
        weakpointCount--;
        if (weakpointCount == 0) {
            //No weakpoints left, we win
            MicrogameController.instance.setVictory(victory: true, final: true);
            yukari.GetComponent<SpriteRenderer>().sprite = sadYukari;
        }

        MicrogameController.instance.playSFX(snapClip,
            AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x));

        /*rope.GetComponent<SpriteRenderer>().sprite = cutRope;
        if (!flipped)
        {
            //Technically not necessary as train should never be in front of peg before microgame ends.
            //But honestly, it was kind of bugging me when doing testing.
            //Anyway this puts the left peg behind the train when the rope has been cut on it.
            rope.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        transform.localScale = new Vector3(0, 0, 0); //Make weak point disappear*/
        this.GetComponentInParent<Animator>().SetTrigger("Cut"); //Play rope cutting animation
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play(); //Play rope cutting particles
        this.GetComponent<CircleCollider2D>().enabled = false;
        ropeDespawn = 0;
    }
}
