using UnityEngine;
using System.Collections;

public class DonationReimu : MonoBehaviour
{

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
	}

	public void reset ()
	{
		//transform.position = new Vector3(0f, transform.position.y, transform.position.z);
		position = 0;
		updatePosition();
		progress = 0;
	}

	void endEarly()
	{
		//ScenarioController.instance.setVictoryDetermined();
	}
	
	void Update ()
	{

		//if (MicrogameTimer.beatsLeft <= 9.5f)
		//{
		//	//Debug.Log(MicrogameTimer.beatsLeft);
		//	ScenarioController.instance.setMicrogameVictory(true, true);
		//}

		updateMovement();
	}

	public void grabCoin(Transform coin)
	{
		progress++;
		if (progress == 3)
		{
			MicrogameController.instance.setVictory(true, true);
		}

		DonationChing ching = textPool.getObjectFromPool().GetComponent<DonationChing>();
		ching.pool = textPool;
		//ching.reset(transform.position + new Vector3(0f, .3f, 0f));
		ching.reset(coin.position);
		ching.GetComponent<TextMesh>().text = progress.ToString();
	}

	void updateMovement()
	{
		if ((Input.GetKeyDown(KeyCode.LeftArrow) && position > minPosition) || Input.GetKeyDown(KeyCode.RightArrow) && position < maxPosition)
		{
			position+= (Input.GetKeyDown(KeyCode.LeftArrow) && position > minPosition) ? -1 : 1;
			updatePosition();
			//if (transform.position.x < minX)
			//	transform.position = new Vector3(minX, transform.position.y, transform.position.z);
			//setFacingRight(false);
			//body.velocity = new Vector2(-1f * moveSpeed, 0f);
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
