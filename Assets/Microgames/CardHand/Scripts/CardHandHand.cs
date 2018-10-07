using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandHand : MonoBehaviour {
    public int speedModifier = 10;

    [Header("Updated by scripts. Changing here does nothing.")]
    public Vector3 target;
    public int dist = 1;

    void Start() {
    }

    void Update() {
        float step = dist * speedModifier * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
