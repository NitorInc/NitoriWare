using UnityEngine;
using System.Collections;

public class DonationReimu : MonoBehaviour
{
	public static int coinsInPlay;

	public int position, minPosition, maxPosition;
	public float minX, maxX, moveSpeed;

	private Rigidbody2D body;

	public float progress;

	public ObjectPool textPool;

	void Awake()
	{
		body = GetComponent<Rigidbody2D>();

		textPool.addToPool(6, true);

		reset();
		coinsInPlay = 0;

	}

	public void reset ()
	{
		position = 0;
		updatePosition();
		progress = 0;
	}

	void Update ()
	{
		if (!MicrogameController.instance.getVictoryDetermined() && coinsInPlay == 0)
			MicrogameController.instance.setVictory(false, true);
		updateMovement();
	}

	public void grabCoin(Transform coin)
	{
		progress++;
		coinsInPlay--;
		if (progress == 3)
		{
			MicrogameController.instance.setVictory(true, true);
		}

		DonationChing ching = textPool.getObjectFromPool().GetComponent<DonationChing>();
		ching.pool = textPool;
		ching.reset(coin.position);
		ching.GetComponent<TextMesh>().text = progress.ToString();
	}

	void updateMovement()
	{
		if ((Input.GetKeyDown(KeyCode.LeftArrow) && position > minPosition) || Input.GetKeyDown(KeyCode.RightArrow) && position < maxPosition)
		{
			position+= (Input.GetKeyDown(KeyCode.LeftArrow) && position > minPosition) ? -1 : 1;
			updatePosition();
		}
		else
		{
			body.velocity = Vector2.zero;
		}
	}

	void updatePosition()
	{
		transform.position = new Vector3(position * 2f, transform.position.y, transform.position.z);
	}

	void setFacingRight(bool facingRight)
	{
			transform.localScale = new Vector3(facingRight ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
	}
}
