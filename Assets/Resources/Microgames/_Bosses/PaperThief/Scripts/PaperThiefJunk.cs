using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefJunk : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private Vector2 xSpeedRange, ySpeedRange;
    [SerializeField]
    private bool staysOnGround;
    [SerializeField]
    private float bounceY, bounceSpeed, torqueMult;
#pragma warning restore 0649

    private bool bounced;

    private Rigidbody2D _rigidBody;

	void Awake()
	{
        _rigidBody = GetComponent<Rigidbody2D>();
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
            }
            else if (staysOnGround)
            {
                transform.position = new Vector3(transform.position.x, bounceY, transform.position.z);
                _rigidBody.velocity = Vector2.zero;
                _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            }
            else
                enabled = false;
        }
	}
}
