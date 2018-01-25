using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareVictimBehavior : MonoBehaviour {

    public Transform victimTransform;
    public float minvictimspawnx;
    public float maxvictimspawnx;
    public float xspawn;

    public Animator rigAnimator;
    public Vibrate vibrate;

    private KogasaScareKogasaBehaviour.State state;


    // Use this for initialization
    void Start ()
    {
        state = KogasaScareKogasaBehaviour.State.Default;
        transform.position = new Vector3(Random.Range(minvictimspawnx, maxvictimspawnx), transform.position.y, transform.position.z);
    }

    public void scare(bool successful, int direction)
    {
        vibrate.vibrateOn = true;
        state = successful ? KogasaScareKogasaBehaviour.State.Victory : KogasaScareKogasaBehaviour.State.Loss;

        rigAnimator.SetTrigger("scare");
        rigAnimator.SetInteger("state", (int)state);
        rigAnimator.SetInteger("direction", direction);
    }

}
