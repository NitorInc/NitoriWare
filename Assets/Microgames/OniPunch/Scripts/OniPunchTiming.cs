using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OniPunchTiming : MonoBehaviour 
{
	public bool success = false;
	// public float sinTest;

	[SerializeField]private float oscSpeed = 1f;
	[SerializeField]private GameObject timingElementsParent;
	[SerializeField]private Image barTarget;
	[SerializeField]private Image barSweetSpot;
	[SerializeField]private Image barBackground;
	[SerializeField]private RectTransform canvas;
	private float initialTargetPos;
	private OniPunchManager manager;
	
	void Start()
	{
		timingElementsParent.SetActive(true);
		initialTargetPos = barTarget.rectTransform.position.x;
		manager = transform.parent.GetComponent<OniPunchManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!success && !MicrogameController.instance.getVictoryDetermined())
		{
			Vector3 newTargetPos = barTarget.rectTransform.position;
			newTargetPos.x = initialTargetPos + (Mathf.Sin(Time.timeSinceLevelLoad * oscSpeed) * ((barBackground.rectTransform.rect.width * 0.5f) - (barTarget.rectTransform.rect.width * 0.5f)) * canvas.localScale.x);// ));
			barTarget.rectTransform.position = newTargetPos;
			// sinTest = Mathf.Sin(Time.timeSinceLevelLoad * oscSpeed) * (barBackground.rectTransform.rect.width * 0.5f);
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (barTarget.rectTransform.position.x < initialTargetPos + barSweetSpot.rectTransform.rect.width * 0.5f &&
				barTarget.rectTransform.position.x > initialTargetPos - barSweetSpot.rectTransform.rect.width * 0.5f)
				{
					manager.winMicrogamePart();
				}
				else
				{
					MicrogameController.instance.setVictory(false);
				}
			}
		}
	}

	public void activate(bool activeState)
	{
		timingElementsParent.SetActive(activeState);
		gameObject.SetActive(activeState);
	}
}
