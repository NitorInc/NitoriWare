using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour
{

	public bool startBlinking;

	public bool blinkOn, spriteOn;

	public float period;

	private float blinkTime;

	private SpriteRenderer spriteRenderer;

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		if (startBlinking)
			enableBlink(true);
		else
			blinkOn = false;
	}


	void Update()
	{
		if (blinkOn)
		{
			blinkTime += Time.deltaTime;
			updateSprite();
		}
		else
			spriteRenderer.enabled = spriteOn;
	}

	/// <summary>
	/// Enables blink
	/// </summary>
	/// <param name="Whether to start with the sprite showing"></param>
	public void enableBlink(bool startOn)
	{
		if (!blinkOn)
		{
			if (startOn)
				blinkTime = 0f;
			else
				blinkTime = period / 2f;
			updateSprite();

			blinkOn = true;
		}
	}
	
	/// <summary>
	/// Disables Blink
	/// </summary>
	/// <param name="Whether to leave the sprite showing"></param>
	public void disableBlink(bool spriteOn)
	{
		if (blinkOn)
		{
			spriteRenderer.enabled = spriteOn;

			blinkOn = false;
		}
	}

	void updateSprite()
	{
		if (spriteRenderer == null)
			spriteRenderer = GetComponent<SpriteRenderer>();

		float t = (blinkTime % period) / period;
		spriteRenderer.enabled = t < .5f;
	}

}