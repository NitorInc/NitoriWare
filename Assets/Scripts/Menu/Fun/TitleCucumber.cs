using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCucumber : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private float collideTime, lifeTime;
    [SerializeField]
    private float minSpeed, maxSpeed, slowDownAcc, speedUpAcc;
    [SerializeField]
    private Rigidbody2D _rigidBody;
#pragma warning restore 0649

    private Vector2 lastMousePosition, flingVelocity;
    private bool grabbed;

	void Start()
    {
        grabbed = false;
        _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), minSpeed);
	}
	
	void Update()
	{
        if (GameMenu.shifting)
        {
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            Vector2 escapeVelocity = MathHelper.getVector2FromAngle(
                ((Vector2)(transform.position - Camera.main.transform.position)).getAngle(), maxSpeed);
            transform.position += (Vector3)escapeVelocity * Time.deltaTime;
            if (CameraHelper.isObjectOffscreen(transform, maxSpeed / 10f))
                Destroy(gameObject);
            return;
        }

        if (!grabbed)
        {
            if (_rigidBody.velocity == Vector2.zero)
                _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), .01f);

            float diff = slowDownAcc * Time.deltaTime;
            if (_rigidBody.velocity.magnitude > minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Max(_rigidBody.velocity.magnitude - diff, minSpeed));
            else if (_rigidBody.velocity.magnitude < minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Min(_rigidBody.velocity.magnitude + diff, minSpeed));

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
                flingVelocity = flingVelocity.resize(Mathf.Min(flingVelocity.magnitude, maxSpeed));
            }
            else
            {
                flingVelocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), minSpeed);
            }
            lastMousePosition = mousePosition;
        }
	}

    public void grab()
    {
        //_rigidBody.bodyType = RigidbodyType2D.Kinematic;
        grabbed = true;
        _rigidBody.freezeRotation = true;
        collideTime = 0f;
        lastMousePosition = CameraHelper.getCursorPosition();
    }

    public void release()
    {
        grabbed = false;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.freezeRotation = false;
        _rigidBody.velocity = flingVelocity;
    }
}
