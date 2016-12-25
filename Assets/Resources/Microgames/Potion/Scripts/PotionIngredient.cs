using UnityEngine;
using System.Collections;

public class PotionIngredient : MonoBehaviour
{
	public float speedToPot;

	public PotionPot pot;
	public SpriteRenderer spriteRenderer;

	public Collider2D theCollider;

	private Rigidbody2D rigidThing;
	private Vector2 grabOffset;

	public State state;
	public enum State
	{
		Idle,
		Grabbed,
		Used,
		SnapBack
	}

	void Awake ()
	{
		theCollider = GetComponent<Collider2D>();
		rigidThing = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		//disableCollision();
	}

	//void disableCollision()
	//{
	//	foreach (PotionIngredient ingredient in pot.ingredients)
	//	{
	//		if (ingredient != this)
	//		{
	//			Physics2D.IgnoreCollision(collider, ingredient.collider);
	//		}
	//	}
	//}
	
	void Update ()
	{
		switch(state)
		{
			case(State.Idle):
				{
					if (pot.state == PotionPot.State.Default && Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(theCollider))
					{
						rigidThing.isKinematic = true;
						grabOffset = (Vector2)(transform.position - CameraHelper.getCursorPosition());
						snapToMouse();
						pot.moveToFront(this);
						state = State.Grabbed;
					}
				}
				break;
			case(State.Grabbed):
				snapToMouse();
				if (pot.state != PotionPot.State.Default || !Input.GetMouseButton(0))
				{
					rigidThing.isKinematic = false;
					state = State.Idle;
				}
				break;
			case (State.Used):
				rigidThing.bodyType = RigidbodyType2D.Static;
				MathHelper.moveTowards2D(transform, pot.transform.position + new Vector3(0f, -.5f, 0f), speedToPot);
				break;
			default:
				break;
		}
	}

	void snapToMouse()
	{
		Vector3 position = CameraHelper.getCursorPosition();
		transform.position = new Vector3(position.x, position.y, transform.position.z) + (Vector3)grabOffset;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		collision(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		collision(other);
	}

	void collision(Collider2D other)
	{
		if (state == State.Idle && other.name == "Front" && pot.state == PotionPot.State.Default)
		{
			state = State.Used;
			rigidThing.isKinematic = true;
			pot.addIngredient(this);
		}
	}
}
