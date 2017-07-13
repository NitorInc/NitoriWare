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
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip grabClip, bounceClip;
#pragma warning restore 0649

    private Vector2 lastPosition, lastMousePosition, flingVelocity;
    private bool grabbed;

	void Start()
    {
        lastPosition = transform.position;
        grabbed = false;
        _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), minSpeed);
	}
	
	void Update()
	{

        if (!grabbed)
        {
            if (_rigidBody.velocity == Vector2.zero)
                _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), .01f);

            float speed = _rigidBody.velocity.magnitude, diff = slowDownAcc * Time.deltaTime;
            if (speed> minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Max(speed - diff, minSpeed));
            else if (speed < minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Min(speed + diff, minSpeed));

            sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
            if ((Mathf.Sign(transform.position.x) == -Mathf.Sign(lastPosition.x))
                || (Mathf.Sign(transform.position.y) == -Mathf.Sign(lastPosition.y)))
            {
                //sfxSource.pitch = Mathf.Lerp(.8f, 1.5f, ((speed - minSpeed) / (maxSpeed - minSpeed)));
                //sfxSource.pitch = Random.Range(.8f, 1f);
                sfxSource.pitch = 1f;
                sfxSource.volume = Mathf.Pow(Mathf.Lerp(0f, 1f, ((speed - minSpeed) / (maxSpeed - minSpeed))), .5f);
                sfxSource.PlayOneShot(bounceClip);
            }


            lastPosition = transform.position;
            

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
        if (GameMenu.shifting)
            return;

        //_rigidBody.bodyType = RigidbodyType2D.Kinematic;
        grabbed = true;
        _rigidBody.freezeRotation = true;
        collideTime = 0f;
        lastMousePosition = CameraHelper.getCursorPosition();

        sfxSource.volume = 1f;
        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(grabClip);
    }

    public void release()
    {
        grabbed = false;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.freezeRotation = false;
        _rigidBody.velocity = flingVelocity;
        lastPosition = transform.position;

        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x);
        sfxSource.pitch = .8f;
        sfxSource.PlayOneShot(grabClip);
    }
}
