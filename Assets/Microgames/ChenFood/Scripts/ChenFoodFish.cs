using UnityEngine;
using System.Collections;

public class ChenFoodFish : MonoBehaviour
{


	public bool eaten, edible;

	public Sprite sprite1, sprite2;
	public float spriteSwitchDistance;
	public float spriteSwitchSpeed;
	public float spriteSwitchIn;


	public Vector2 lastPosition;

	private SpriteRenderer spriteRenderer;

	public Transform eyes;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}


	void LateUpdate()
	{

		updatePosition();

		Vector2 diff = Vector2.zero;
		if (diff.magnitude >= spriteSwitchIn)
		{
			if (spriteRenderer.sprite == sprite1)
				spriteRenderer.sprite = sprite2;
			else
				spriteRenderer.sprite = sprite1;
			spriteSwitchIn += spriteSwitchDistance;
		}
		else
		{
			spriteSwitchIn -= diff.magnitude + (spriteSwitchSpeed * Time.deltaTime);
		}

		lastPosition = (Vector2)transform.position;
	}

	public void updatePosition()
	{
		if  (eaten)
		{
			transform.position = new Vector3(-7f, 7.5f, transform.position.z);
			return;
		}

		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		collide(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		collide(other);
	}

	void collide(Collider2D other)
	{
		if (edible && !eaten && other.name == "Chen")
		{
			eaten = true;
			ChenFoodChen cat = other.GetComponent<ChenFoodChen>();
			cat.leapSource.PlayOneShot(cat.purrClip);
			MicrogameController.instance.setVictory(false, true);
		}
	}
}
