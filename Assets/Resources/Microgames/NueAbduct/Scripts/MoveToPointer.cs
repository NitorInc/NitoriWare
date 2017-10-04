using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPointer : MonoBehaviour {

    public Transform target;
    public float speed = 2.0f;

    void Update() {
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float step = speed * Time.deltaTime;
        target.transform.position = Vector2.MoveTowards(target.transform.position, targetPos, step);
    }
}
