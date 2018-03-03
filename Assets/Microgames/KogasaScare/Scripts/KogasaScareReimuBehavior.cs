using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaScareReimuBehavior : MonoBehaviour
{
    [SerializeField]
    private Vibrate vibrate;

    void onScare(bool scared)
    {
        vibrate.vibrateOn = true;
    }
}
