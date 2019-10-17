using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumiaRescueTreeController : MonoBehaviour {

    [SerializeField]
    private ParticleSystem MapleLeavesFX;

    public void PlayMapleLeavesFX() {
        MapleLeavesFX.Play();
    }
}
