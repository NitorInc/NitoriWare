using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookTarget_rot : MonoBehaviour
{

    public Transform center;
    public SineWave sineWave;
    public Vector3 axis = Vector3.up;


    public float radiusSpeed = 0.5f;
    public float rotationSpeed = 80.0f;
    public bool randomizeDirection = true;

    private float radius;

    void Start()
    {
        calculateRadius();
        transform.position = (transform.position - center.position).normalized * radius + center.position;
        if (randomizeDirection)
            rotationSpeed *= Random.Range(0, 2) == 1 ? 1f : -1f;
    }

    void Update()
    {
        calculateRadius();
        if (MicrogameController.instance.getVictoryDetermined())
        {
            enabled = false;
            sineWave.enabled = true;
            sineWave.yOffset = 0f;
            sineWave.resetStartPosition();
            sineWave.resetCycle();
            return;
        }

        transform.RotateAround(center.position, axis, rotationSpeed * Time.deltaTime);
    }

    void calculateRadius()
    {
        Vector2 dist = new Vector2(transform.position.x, transform.position.z) - new Vector2(center.position.x, center.position.z);
        radius = dist.magnitude;
    }
}