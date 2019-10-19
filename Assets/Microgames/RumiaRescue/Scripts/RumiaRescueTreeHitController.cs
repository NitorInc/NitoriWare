using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueTreeHitController : MonoBehaviour {

    [SerializeField]
    private Animator ani;

    private static bool canHit = true;
    public static bool CanHit {
        get { return canHit; }
        set { canHit = value; }
    }

    private static bool hasHitted = false;
    

    private void Start() {
        canHit = true;
        hasHitted = false;
    }

    private void OnTriggerEnter2D(Collider2D collider2D) {
        if (canHit == false)
            return;
        if (hasHitted == true)
            return;
        hasHitted = true;

        collider2D.transform.parent.SendMessage("WhenRumiaHitTree");
        ani.SetTrigger("Hitted");
        MainCameraSingleton.instance.SendMessage("setScreenShake",0.3f);

        MicrogameController.instance.setVictory(false);
    }
}
