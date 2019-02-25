using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OniPunchCharging : MonoBehaviour 
{
	public bool success = false;

	[SerializeField]private Image chargeBar;
	[SerializeField]private float chargeMax = 100f;
	[SerializeField]private float chargeIncrement = 5f;
	[SerializeField]private float chargeDecrementOverTime = 45f;

	private float chargeValue = 0f;
	private int lastButtonPressedIndicator = 0;		//1 left - 2 right - 0 none
	private OniPunchManager manager;
	
	void Start()
	{
		manager = transform.parent.GetComponent<OniPunchManager>();
	}

	void Update () 
	{
		if (!success && !MicrogameController.instance.getVictoryDetermined())
		{
			if (chargeValue >= 0f)
			{
				chargeValue = Mathf.Max(chargeValue - (Time.unscaledDeltaTime * chargeDecrementOverTime), 0f);
			}
			
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				charge(1);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				charge(2);
			}

			updateUI();
		}
	}

	void charge(int direction)
	{
		chargeValue += direction != lastButtonPressedIndicator ? chargeIncrement * 2f : chargeIncrement;
		lastButtonPressedIndicator = direction;
		if (chargeValue >= chargeMax)
		{
			chargeValue = Mathf.Clamp(chargeValue, 0f, chargeMax);
			success = true;
			updateUI();
			manager.winMicrogamePart();
		}
	}

	void updateUI()
	{
		chargeBar.fillAmount = Mathf.InverseLerp(0, chargeMax, chargeValue);
	}	

	public void activate(bool activeState)
	{
		chargeBar.transform.parent.gameObject.SetActive(activeState);
		gameObject.SetActive(activeState);
	}
}
