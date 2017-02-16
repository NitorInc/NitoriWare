using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaShakeSuika : MonoBehaviour
{
	[SerializeField]
	private float health;
	private bool onBottle = true;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;
	public SpriteRenderer spriteRenderer
	{
		get { return _spriteRenderer; }
		set { _spriteRenderer = value; }
	}

	private Rigidbody2D _rigidBody;

	void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		
	}

	public void setHealth(float health)
	{
		this.health = health;

		if (onBottle && health < 0)
		{
			onBottle = false;
			fling();
		}
	}

	void fling()
	{
		transform.parent = null;
		_rigidBody.bodyType = RigidbodyType2D.Dynamic;
		_rigidBody.velocity = MathHelper.getVectorFromAngle2D(Random.Range(45, 135), Random.Range(5, 15));

		_spriteRenderer.GetComponent<Vibrate>().vibrateOn = false;
	}

	public float getHealth()
	{
		return health;
	}

}
