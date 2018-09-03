using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareVictimBehavior : MonoBehaviour
{

    public Transform victimTransform;
    public Animator rigAnimator;
    private KogasaScareKogasaBehaviour.State state;

    // Use this for initialization
    void Start ()
    {
        state = KogasaScareKogasaBehaviour.State.Default;
    }

    public void scare(bool successful, int direction)
    {
        state = successful ? KogasaScareKogasaBehaviour.State.Victory : KogasaScareKogasaBehaviour.State.Loss;

        rigAnimator.SetTrigger("scare");
        rigAnimator.SetInteger("state", (int)state);
        rigAnimator.SetInteger("direction", direction);

        SendMessage("onScare", successful, SendMessageOptions.DontRequireReceiver);
    }

}
