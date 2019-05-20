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

    int ropeDespawn = -1;

    // Use this for initialization
    void Start() {
        //Place weak points on rope
        weakpointCount = 2;
        //DATA: Values selected to ensure randomization places weak point on rope
        float startx = Random.Range(-2f, 2f);
        float yfactor = -0.21f;
        if (flipped) {
            yfactor *= -1;
        }
        Vector3 position = new Vector3(startx, (startx - 0.5f) * yfactor, 0);
        transform.localPosition = position;
        this.GetComponentInParent<Animator>().enabled = false; //We don't want to animate it until the player cuts it
    }

    // Update is called once per frame
    void Update() {
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
        weakpointCount--;
        if (weakpointCount == 0) {
            //No weakpoints left, we win
            MicrogameController.instance.setVictory(victory: true, final: true);
            yukari.GetComponent<SpriteRenderer>().sprite = sadYukari;
        }
        /*rope.GetComponent<SpriteRenderer>().sprite = cutRope;
        if (!flipped)
        {
            //Technically not necessary as train should never be in front of peg before microgame ends.
            //But honestly, it was kind of bugging me when doing testing.
            //Anyway this puts the left peg behind the train when the rope has been cut on it.
            rope.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
        transform.localScale = new Vector3(0, 0, 0); //Make weak point disappear*/
        this.GetComponentInParent<Animator>().enabled = true; //Play rope cutting animation
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play(); //Play rope cutting particles
        ropeDespawn = 0;
    }
}
