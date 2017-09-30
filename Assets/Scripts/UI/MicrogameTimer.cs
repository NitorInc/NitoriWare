using UnityEngine;
using System.Collections;

public class MicrogameTimer : MonoBehaviour
{

	public static MicrogameTimer instance;

	public float countdownScale;

	public float beatsLeft;

	public bool disableDisplay;

	public AudioSource tick;

	public Transform[] chain;
	public Transform key;
	public SpriteRenderer gear, countdown;
	public Sprite[] countdownNumbers;

	private Vector3 hold;

	void Awake()
	{
		instance = this;

		chain = new Transform[8];
		Transform hold = transform.Find("Chain");
		for (int i = 0; i < chain.Length; i++)
		{
			chain[i] = hold.Find("Chain " + i.ToString());
		}
		key = hold.Find("Key");
		gear = transform.Find("Gear").GetComponent<SpriteRenderer>();
		countdown = transform.Find("Countdown").GetComponent<SpriteRenderer>();
	}

	public void invokeTick()
	{
		for (int i = 0; i < 3; i++)
		{
			Invoke("playTick", (beatsLeft - (i + 2)) * StageController.beatLength);
		}
	}

	private void playTick()
	{
		tick.pitch = Time.timeScale;
		tick.Play();
	}

	void Update()
	{
		beatsLeft -= Time.deltaTime / StageController.beatLength;
	}

	void LateUpdate()
	{
		if (!disableDisplay && beatsLeft < 9f && beatsLeft > 0.0f)
		{

			int beat = (int)Mathf.Floor(beatsLeft);

			gear.enabled = countdown.enabled = true;
			key.gameObject.SetActive(beat != 0);


			for (int i = 0; i < chain.Length; i++)
			{
				chain[i].gameObject.SetActive(i + 1 <= beat);

				hold = chain[i].localPosition;

				setX(chain[i], -9f + ((float)i * 2f) - getChainOffset(beat));

				if (i + 1 == beat)
					setX(key, chain[i].localPosition.x + 1.0f);
			}

			if (beat < 4)
				countdown.sprite = countdownNumbers[beat];
			else
				countdown.sprite = null;
			float scale = getCountdownScale();
			countdown.transform.localScale = new Vector3(scale / transform.parent.localScale.x, scale / transform.parent.localScale.y, 1f);

			gear.transform.rotation = Quaternion.Euler(0f, 0f, getGearAngle(beat) * Mathf.Rad2Deg);


			float size = .25f;
			if (beat == 0)
			{
				float gearFadeDuration = .5f;

				gear.transform.localScale = new Vector3(Mathf.Lerp(size, 0f, (1f - beatsLeft) / gearFadeDuration),
					Mathf.Lerp(size, size * 2f, (1f - beatsLeft) / gearFadeDuration), 1f);
				gear.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, (1f - beatsLeft) / gearFadeDuration));
			}
			else
			{
				gear.transform.localScale = new Vector3(size, size, 1f);
				gear.color = Color.white;
			}

		}
		else
		{
			for (int i = 0; i < chain.Length; i++)
			{
				chain[i].gameObject.SetActive(false);
			}
			key.gameObject.SetActive(false);
			gear.enabled = false;
			countdown.enabled = false;
		}

	}

	private float getCountdownScale()
	{
		float t = 1 - (beatsLeft % 1);


		float delay = .25f, growDuration = .25f;

		return Mathf.Lerp(0f, countdownScale, (t - delay) / growDuration);
	}

	private static float[] chainOffsets = {0f, .03f, -.12f, -.04f, 0f, -.15f, -.05f, 0f};

	private float getChainOffset(int beat)
	{
		if (beat >= 0 && beat < 8) return chainOffsets[beat];
		else return .05f;
	}

	private float getGearAngle(int beat)
	{
		//return beat == 0 ? 0 : (float)(7 - beat) * Mathf.PI * 3f / 8f;

		float angle = Mathf.Lerp(0f, Mathf.PI * 24f * 8f / 7f, (float)beat / 8f);
		//angle *= Mathf.PI / 8f;
		//angle = Mathf.Floor(angle);
		//angle /= Mathf.PI / 8f;
		return angle;
	}

	private void setX(Transform trans, float x)
	{
		hold = trans.localPosition;
		hold.x = x;
		trans.localPosition = hold;
	}
}
