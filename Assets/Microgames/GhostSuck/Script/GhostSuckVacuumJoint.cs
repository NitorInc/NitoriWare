using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckVacuumJoint : MonoBehaviour {

    private float angle;
    [SerializeField]
    private float angleoffset;
    [SerializeField]
    private GameObject tiltreference;
    [SerializeField]
    private float delay;
    [SerializeField]
    private bool updated;
    public float motorSpeedMult;
    public HingeJoint2D joint;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Transform tail;
    private Vector2 trajectory;
    private Vector3 lastMousePosition;


    private SpriteRenderer spriteRenderer;

    //returns angle value based on cursor position
    void Update()
    {
        Vector3 relative = CameraHelper.getCursorPosition();
        
            angle = ((Vector2)(transform.position - relative)).getAngle() - angleoffset;
            updateBodyRotation();
        setAngle();
        updateRotation();
        lastMousePosition = transform.position;

    }
    //uses angle value to determine where vacuum should be pointed, geometry gives me a headache
    private void setAngle()
    {
        if (angle > 180f)
            angle -= 360f;
        if (angle < 0f)
            angle += 360f;
        if (130f < angle && angle < 180f)
            angle = 180f - angle;
        if (180f < angle && angle < 230f)
            angle = 180f - angle;

    }
    //sets a child object at the cursor to the relative angle for later use
    void updateRotation()
    {
        tiltreference.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        if (updated == true)
            updated = false;
    }
    //rotates vacuum along a clamped joint
    void updateBodyRotation()
    {
        float zRotation = tiltreference.transform.eulerAngles.z;
        if (zRotation > 180f)
            zRotation -= 360f;
        zRotation = Mathf.Clamp(zRotation, joint.limits.min, joint.limits.max);
        if (zRotation < 0f)
            zRotation += 360f;
        tail.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}
