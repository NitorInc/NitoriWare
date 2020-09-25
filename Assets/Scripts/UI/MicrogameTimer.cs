using UnityEngine;
using System.Collections;

public class MicrogameTimer : MonoBehaviour
{
	public float countdownScale;
	public bool disableDisplay;

	public AudioSource tick;

	public Transform[] chain;
	public Transform key;
	public SpriteRenderer gear, countdown;
	public Sprite[] countdownNumbers;

	private Vector3 hold;
	private Microgame.Session session;

	public void StartPlayback(Microgame.Session session)
    {
		this.session = session;
		StartCoroutine(PlayTicks(session));
    }


	IEnumerator PlayTicks(Microgame.Session session)
	{
		if (session.BeatsRemaining < float.PositiveInfinity)
        {
			yield return new WaitForSeconds((float)(session.BeatsRemaining - 4d) * (float)Microgame.BeatLength);
			if (!session.IsEndingEarly)
			{
				playTick();
				yield return new WaitForSeconds((float)Microgame.BeatLength);
				playTick();
				yield return new WaitForSeconds((float)Microgame.BeatLength);
				playTick();
			}
		}

	}

	private void playTick()
	{
		tick.pitch = 1f;
		tick.Play();
	}

	void LateUpdate()
	{
		if (session != null && !disableDisplay && session.BeatsRemaining < 9f && session.BeatsRemaining > 0.0f)
		{
			var beatsLeft = session.BeatsRemaining;
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
		float t = 1 - (session.BeatsRemaining % 1);


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
		return Mathf.Lerp(0f, Mathf.PI * 24f * 8f / 7f, (float)beat / 8f);
	}

	private void setX(Transform trans, float x)
	{
		hold = trans.localPosition;
		hold.x = x;
		trans.localPosition = hold;
	}
}
