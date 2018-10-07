using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandHand : MonoBehaviour {
    public Vector3 target;
    public int dist = 1;

    public int speedModifier = 10;

    void Start() {
    }

    void Update() {
        float step = dist * speedModifier * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
