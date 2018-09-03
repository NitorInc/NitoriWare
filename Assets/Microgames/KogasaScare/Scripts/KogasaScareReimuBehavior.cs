using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareReimuBehavior : MonoBehaviour
{
    [SerializeField]
    private Vibrate vibrate;
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private KogasaScareKogasaBehaviour kogasa;

    void onSpawnSet()
    {
        rigAnimator.SetInteger("direction", transform.position.x > kogasa.transform.position.x ? 1 : -1);
    }

    void onScare(bool scared)
    {
        vibrate.vibrateOn = true;
    }
}
