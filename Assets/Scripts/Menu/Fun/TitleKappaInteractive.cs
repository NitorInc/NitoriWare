using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleKappaInteractive : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private ParticleSystem eatParticles;
    [SerializeField]
    private float collideTime, lifeTime, clickSpeed;
    [SerializeField]
    private float minSpeed, maxSpeed, slowDownAcc, speedUpAcc;
    [SerializeField]
    private float eatGrowScale;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip eatClip;
    [SerializeField]
    private TitleFloatingInteractive floatingInteractive;
#pragma warning restore 0649

    private Vector2 lastMousePosition, flingVelocity;
    private Vector3 initialScale;
    private bool grabbed;

    void Start()
    {
        grabbed = false;
        initialScale = transform.localScale;
        //_rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), minSpeed);
    }

    void Update()
    {

        if (!grabbed)
        {
            if (_rigidBody.velocity == Vector2.zero)
                _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), .01f);

            float speed = _rigidBody.velocity.magnitude, diff = slowDownAcc * Time.deltaTime;
            if (speed > minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Max(speed - diff, minSpeed));
            else if (speed < minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Min(speed + diff, minSpeed));


            //if (_rigidBody.velocity == Vector2.zero)
            //    _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), minVelocity);
            //else
            //    _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Clamp(_rigidBody.velocity.magnitude - diff, minVelocity, maxVelocity));
        }
        else
        {
            Vector2 mousePosition = CameraHelper.getCursorPosition();
            if (mousePosition - lastMousePosition != Vector2.zero)
            {
                flingVelocity = (mousePosition - lastMousePosition) / Time.deltaTime;
                flingVelocity = flingVelocity.resize(Mathf.Clamp(flingVelocity.magnitude, minSpeed, maxSpeed));
            }
            lastMousePosition = mousePosition;
        }
        

        if (CameraHelper.isMouseOver(_collider2D))
            click();
    }

    void click()
    {
        _rigidBody.velocity = ((Vector2)transform.position - (Vector2)CameraHelper.getCursorPosition()).resize(clickSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Cucumber"))
        {
            Destroy(collision.collider.gameObject);
            transform.localScale *= eatGrowScale;
            sfxSource.PlayOneShot(eatClip);
            eatParticles.Play();
        }
    }
}
