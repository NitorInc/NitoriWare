using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleGear : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private float minSpeed, maxSpeed, slowDownAcc, speedUpAcc;
    [SerializeField]
    private Vector2 zoomSpeedBounds, startRotateBounds, zoomRotateBounds;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private Collider2D clickCollider;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip hopClip, zoomClip;
    [SerializeField]
    private Animator hopAnimator;
#pragma warning restore 0649

    private float zoomAngle;
    private bool hopping;

    void Start()
    {
        _rigidBody.AddTorque(MathHelper.randomRangeFromVector(startRotateBounds) * (_rigidBody.velocity.x >= 0f ? -1f : 1f));
    }

    void Update()
    {
        if (!hopping)
        {
            if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
                press();
            else
            {
                if (_rigidBody.velocity == Vector2.zero)
                    _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), .01f);

                float speed = _rigidBody.velocity.magnitude, diff = slowDownAcc * Time.deltaTime;
                if (speed > minSpeed)
                    _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Max(speed - diff, minSpeed));
                else if (speed < minSpeed)
                    _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Min(speed + diff, minSpeed));
            }
        }
    }

    void press()
    {
        zoomAngle = _rigidBody.velocity.getAngle();
        _rigidBody.velocity = Vector2.zero;

        Transform child = transform.GetChild(0);
        child.transform.localRotation = Quaternion.Euler(0f, 0f,
            child.transform.rotation.eulerAngles.z + transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.identity;

        hopping = true;
        hopAnimator.SetBool("Hop", hopping);
        clickCollider.enabled = false;
    }

    public void spin()
    {
        _rigidBody.freezeRotation = true;
        _rigidBody.freezeRotation = false;
        _rigidBody.AddTorque(MathHelper.randomRangeFromVector(zoomRotateBounds) * (_rigidBody.velocity.x >= 0f ? -1f : 1f));

        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(hopClip);
    }

    public void fling()
    {
        _rigidBody.velocity = MathHelper.getVector2FromAngle(zoomAngle, MathHelper.randomRangeFromVector(zoomSpeedBounds));

        sfxSource.volume = 1f;
        sfxSource.PlayOneShot(zoomClip);
        hopping = false;
        hopAnimator.SetBool("Hop", hopping);
        clickCollider.enabled = true;

    }
}
