using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCucumber : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private float collideTime, lifeTime;
    [SerializeField]
    private float minSpeed, maxSpeed, slowDownAcc, speedUpAcc;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip grabClip;
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
	}

    public void grab()
    {
        if (GameMenu.shifting)
            return;

        flingVelocity = _rigidBody.velocity;
        grabbed = true;
        _rigidBody.freezeRotation = true;
        collideTime = 0f;
        lastMousePosition = CameraHelper.getCursorPosition();
        
        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x, 1f);
        sfxSource.pitch = 1f;
        sfxSource.PlayOneShot(grabClip);

        transform.localScale = initialScale * 1.2f;
    }

    public void release()
    {
        grabbed = false;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.freezeRotation = false;
        _rigidBody.velocity = flingVelocity;
        floatingInteractive.lastVelocity = _rigidBody.velocity;

        sfxSource.panStereo = AudioHelper.getAudioPan(transform.position.x, 1f);
        sfxSource.pitch = .8f;
        sfxSource.PlayOneShot(grabClip);

        transform.localScale = initialScale * 1f;
    }
}
