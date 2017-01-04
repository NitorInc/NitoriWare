using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBandLight : MonoBehaviour
{

	private HSBColor hsbColor;
	private Light _light;
	private float cycleSpeed;

	void Awake()
	{
		shuffleColor();
		_light = GetComponent<Light>();
		cycleSpeed = Random.Range(.2f, .5f) * (Random.Range(0, 1) == 0 ? 1f : -1f);
		updateColor();
	}

	void Update()
	{
		if (!MicrogameController.instance.getVictory() && MicrogameController.instance.getVictoryDetermined())
		{
			_light.enabled = false;
			return;
		}

		hsbColor.h += cycleSpeed * Time.deltaTime;// *(MicrogameController.instance.getVictory() ? 4f : 1f);
		Debug.Log(hsbColor.h);
		
		if (hsbColor.h >= 1f)
			hsbColor.h -= 1f;
		else if (hsbColor.h < 0f)
			hsbColor.h += 1f;

		updateColor();
	}

	public void onHit()
	{
		shuffleColor();
	}

	public void onVictory()
	{
		shuffleColor();
		cycleSpeed *= 4f;
	}

	void shuffleColor()
	{
		hsbColor = new HSBColor(Random.Range(0f, 1f), 1f, 2f);
	}

	void updateColor()
	{
		_light.color = HSBColor.ToColor(hsbColor);
	}
}
