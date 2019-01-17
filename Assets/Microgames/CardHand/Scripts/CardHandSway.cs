using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandSway : MonoBehaviour {
    public float minLimit = 0f;
    [SerializeField]
    public float maxLimit = 5f;
    [SerializeField]
    public float speed = 2;

    bool direction = true;  // false: left, true: right
    float easeStarted;
    float lastZ;

	void Start () {
        easeStarted = Time.fixedTime;
	}

    float EaseInOutQuad(float t, float b, float c, float d) {
        t /= d / 2f;
        if (t < 1f) return c / 2f * t * t + b;

        t -= 1f;
        return -c / 2f * (t * (t - 2f) - 1f) + b;
    }

    void Update () {
        float end = direction ? maxLimit : minLimit;
        float start = direction ? minLimit : maxLimit;
        
        float z = EaseInOutQuad(Time.fixedTime - easeStarted, start, end - start, speed);

        if (Time.fixedTime - easeStarted >= speed) {
            direction = !direction;
            easeStarted = Time.fixedTime;
        }
        transform.eulerAngles = new Vector3(0, 0, z);

        lastZ = z;
    }
}
