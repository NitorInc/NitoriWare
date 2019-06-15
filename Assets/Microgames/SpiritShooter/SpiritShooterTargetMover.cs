using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritShooterTargetMover : MonoBehaviour {

    GameObject startPoint;
    GameObject endPoint;
    public AnimationCurve curve;
    public float fadeSpeed = 1.0f;
    float t = 0;
    bool bIsMoving = false;

    public void Initialize(GameObject start, GameObject end) {
        startPoint = start;
        endPoint = end;
        transform.parent.position = startPoint.transform.position;
    }

    public void StartMoving() {
        bIsMoving = true;
    }

    public void Flip() {
        GetComponent<SpiritShooterTargetUnit>().Flip();
    }

    void Update () {
        if (bIsMoving) {
            transform.position = Vector3.Lerp(startPoint.transform.position, endPoint.transform.position, curve.Evaluate(t));
            t += Time.deltaTime * fadeSpeed;
            if (t >= 1.0f) {
                Flip();
                bIsMoving = false;
            }
        }
    }
}
