using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaShakeSuika : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
	private float health;
    [SerializeField]
    private Sprite middleSprite, rightSprite, fallSprite;
    [SerializeField]
    private Vibrate vibrate;
    [SerializeField]
    private AudioClip flingClip;
    [SerializeField]
    private float minTimePerFlingSound;
#pragma warning restore 0649


    private bool onBottle = true;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;
	public SpriteRenderer spriteRenderer
	{
		get { return _spriteRenderer; }
		set { _spriteRenderer = value; }
	}

	private Rigidbody2D _rigidBody;
    private int direction;

	void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
	}

    public void setFacing(int direction)
    {
        this.direction = direction;
        if (direction == 0)
            _spriteRenderer.sprite = middleSprite;
        else
        {
            _spriteRenderer.sprite = rightSprite;
            if (direction < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        vibrate.vibrateSpeed *= Random.Range(.5f, 1.5f);
    }

	public void setHealth(float health, Vector2 velocity)
	{
		this.health = health;

		if (onBottle && health < 0)
		{
			onBottle = false;
			fling(velocity);
		}
	}

	void fling(Vector2 velocity)
	{
		transform.parent = null;
		_rigidBody.bodyType = RigidbodyType2D.Dynamic;
		_rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(60, 90), Random.Range(5, 15));
        if (direction < 0 || (direction == 0 && MathHelper.randomBool()))
            _rigidBody.velocity = new Vector2(-_rigidBody.velocity.x, _rigidBody.velocity.y);

		_spriteRenderer.GetComponent<Vibrate>().vibrateOn = false;

        spriteRenderer.sprite = fallSprite;
        if (_rigidBody.velocity.x < 0f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(15f, 30f));
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localRotation = Quaternion.Euler(0f, 0f, -Random.Range(15f, 30f));
        }
        _rigidBody.AddTorque(15f + (30f * -_rigidBody.velocity.x * Random.Range(.5f, 1f)));


        velocity /= 5f;
        velocity = velocity.resize(Mathf.Min(velocity.magnitude, 10f));
        _rigidBody.velocity += velocity;

        if (SuikaShakeBottle.flingSoundCooldown <= 0f)
        {
            MicrogameController.instance.playSFX(flingClip, MicrogameController.instance.getSFXSource().panStereo, Random.Range(1f, 1.05f));
            SuikaShakeBottle.flingSoundCooldown += minTimePerFlingSound;
        }
    }

	public float getHealth()
	{
		return health;
    }

    public void generateOffset(Collider2D spawnCollider)
    {
        Vector2 bounds = spawnCollider.bounds.extents;
        transform.position += spawnCollider.transform.localPosition + (Vector3)spawnCollider.offset
            + new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y), 0f);
    }

}
