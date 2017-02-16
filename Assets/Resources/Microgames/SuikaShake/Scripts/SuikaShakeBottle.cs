using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaShakeBottle : MonoBehaviour
{
	[SerializeField]
	private float health, minSpeed, progressMult;
	[SerializeField]
	private int healthPerSuika;
	[SerializeField]
	private Vector2 suikaOffsetXBounds, suikaOffsetYBounds;
	[SerializeField]
	private GameObject suikaPrefab;

	private bool _pauseBuffer = false;
	public bool pauseBuffer
	{
		set { _pauseBuffer = value; }
		get { return _pauseBuffer; }
	}

	private SuikaShakeSuika[] suikas;
	private Vector2 lastCursorPosition;

	void Start ()
	{
		suikas = new SuikaShakeSuika[(int)(health / healthPerSuika)];
		for (int i = 0; i < suikas.Length; i++)
		{
			suikas[i] = Instantiate(suikaPrefab, transform.position + generateSuikaOffset(), Quaternion.identity).GetComponent<SuikaShakeSuika>();
			suikas[i].transform.parent = transform;
			suikas[i].spriteRenderer.sortingOrder = i + 1;
		}
		lastCursorPosition = CameraHelper.getCursorPosition();
		pauseBuffer = true;
	}

	void Update()
	{
		Vector2 currentCursorPosition = CameraHelper.getCursorPosition();

		if (health < 0)
		{
			if (MathHelper.moveTowards2D(transform, Vector2.zero, 30f))
				enabled = false;
			return;
		}
		if (pauseBuffer)
		{
			pauseBuffer = false;
			lastCursorPosition = currentCursorPosition;
			return;
		}

		float diff = (currentCursorPosition - lastCursorPosition).magnitude;
		if (diff / Time.deltaTime >= minSpeed)
			lowerHealth(diff * progressMult);

		lastCursorPosition = currentCursorPosition;
	}

	void lowerHealth(float amount)
	{
		health -= amount;
		for (int i = 0; i < suikas.Length; i++)
		{
			suikas[i].setHealth(health - (i * healthPerSuika));
		}

		if (health <= 0)
		{
			GetComponent<FollowCursor>().enabled = false;
		}
	}

	Vector3 generateSuikaOffset()
	{
		return new Vector3(Random.Range(suikaOffsetXBounds.x, suikaOffsetXBounds.y) * transform.localScale.x,
			Random.Range(suikaOffsetYBounds.x, suikaOffsetYBounds.y) * transform.localScale.y, 0f);
	}
}
