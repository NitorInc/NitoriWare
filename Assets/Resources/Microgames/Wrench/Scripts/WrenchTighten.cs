using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WrenchTighten : MonoBehaviour 
{

	public float upSpeed, downSpeed, maxRotation, scaleSpeed, minScale, maxScale, boltRotation, progressMult;
	public int cyclesNeeded;
	public bool arrowIndicator, fastening, finished;
	public UnityEvent onVictory;

	public Transform bolt, screw;
	public Vibrate handVibrate;
	public Blink arrowBlink;

	private float minRotation;
	private int cyclesLeft;
	private bool moving;

	void Start ()
	{
		reset();
	}

	public void reset()
	{
		moving = false;
		minRotation = maxRotation - (Mathf.PI / 3f);
		setRotation(maxRotation);
		fastening = true;
		setScale(minScale);
		setBoltRotation(getRotation() + boltRotation);
		cyclesLeft = cyclesNeeded;
		finished = false;
		arrowBlink.disableBlink(false);
	}
	
	void Update ()
	{
		if (!finished)
			updateFasten();
		updateScale();
	}

	float getScrewHeight()
	{
		if (cyclesLeft == 0f)
			return 0f;
		else if (fastening)
			return ((float)(cyclesLeft - 1) * progressMult) + (getCycleProgress() * progressMult);
		else
			return (float)cyclesLeft * progressMult;
	}

	float getCycleProgress()
	{
		return (getRotation() - minRotation)/(maxRotation - minRotation);
	}

	void finish()
	{
		handVibrate.vibrateOn = false;
		MicrogameController.instance.setVictory(true, true);
		screw.transform.localPosition = new Vector3(0f, getScrewHeight(), 0f);
		finished = true;
		onVictory.Invoke();
	}

	void updateFasten()
	{
		if (fastening)
		{
			if (Input.GetKeyDown(KeyCode.DownArrow))
				moving = true;
			if (moving)
			{
				float diff = downSpeed * Time.deltaTime;
				if (getRotation() - diff <= minRotation)
				{
					cyclesLeft--;
					moving = false;
					fastening = false;
					setRotation(minRotation);
					if (cyclesLeft == 0)
					{
						finish();
						return;
					}
					else if (arrowIndicator)
							arrowBlink.enableBlink(true);
				}
				else
					setRotation(getRotation() - diff);
			}
			setBoltRotation(getRotation() + boltRotation);
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				moving = true;
			if (moving)
			{
				float diff = upSpeed * Time.deltaTime;
				if (getRotation() + diff >= maxRotation)
				{
					moving = false;
					fastening = true;
					arrowBlink.disableBlink(false);
					setRotation(maxRotation);
				}
				else
					setRotation(getRotation() + diff);
			}
		}


		handVibrate.vibrateOn = fastening;
		screw.transform.localPosition = new Vector3(0f, getScrewHeight(), 0f);
	}

	void snapWrench()
	{

		float increment = Mathf.PI / 3f, diff = ((getBoltRotation() % increment) - (getRotation() % increment));

		//if (diff < 0f)
		//	diff += increment;

		setRotation(getRotation() + diff);

	}

	void updateScale()
	{
		if (fastening && getScale() > minScale)
		{
			float diff = scaleSpeed * Time.deltaTime;
			if (getScale() - diff <= minScale)
			{
				setScale(minScale);
			}
			else
				setScale(getScale() - diff);
		}
		else if (!fastening && getScale() < maxScale)
		{
			float diff = scaleSpeed * Time.deltaTime;
			if (getScale() + diff >= maxScale)
			{
				setScale(maxScale);
			}
			else
				setScale(getScale() + diff);
		}

	}

	float getRotation()
	{
		return transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
	}

	void setRotation(float rotation)
	{
		transform.rotation = Quaternion.Euler(0f, 0f, rotation * Mathf.Rad2Deg);
	}

	float getScale()
	{
		return transform.localScale.x;
	}

	void setScale(float scale)
	{
		transform.localScale = new Vector3(scale, scale, transform.position.x);
	}

	float getBoltRotation()
	{
		return bolt.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
	}

	void setBoltRotation(float rotation)
	{
		bolt.transform.rotation = Quaternion.Euler(0f, 0f, rotation * Mathf.Rad2Deg);
	}

}
