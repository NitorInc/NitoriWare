using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefJunk : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Vector2 xSpeedRange, ySpeedRange;
    [SerializeField]
    private bool staysOnGround;
    [SerializeField]
    private float bounceY, bounceSpeed, torqueMult;
    [SerializeField]
    private AudioClip bounceClip;
#pragma warning restore 0649

    private bool bounced;

    private Rigidbody2D _rigidBody;
    private AudioSource _audioSource;

	void Awake()
	{
        _rigidBody = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
	}

    void Start()
    {
        _rigidBody.velocity = new Vector2(MathHelper.randomRangeFromVector(xSpeedRange), MathHelper.randomRangeFromVector(ySpeedRange));
        _rigidBody.AddTorque(_rigidBody.velocity.x * -torqueMult);
    }

    void Update()
	{
        if (transform.position.y < bounceY && _rigidBody.velocity.y < 0f)
        {
            if (!bounced)
            {
                transform.position = new Vector3(transform.position.x, bounceY, transform.position.z);
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, bounceSpeed);
                bounced = true;
                _audioSource.PlayOneShot(bounceClip);
            }
            else if (staysOnGround)
            {
                transform.position = new Vector3(transform.position.x, bounceY, transform.position.z);
                _rigidBody.velocity = Vector2.zero;
                _rigidBody.bodyType = RigidbodyType2D.Kinematic;
                _audioSource.PlayOneShot(bounceClip, .5f);
            }
            else
                enabled = false;
        }
        _audioSource.pitch = 1f;
        _audioSource.panStereo = AudioHelper.getAudioPan(transform.position.x) * .5f;
	}
}
