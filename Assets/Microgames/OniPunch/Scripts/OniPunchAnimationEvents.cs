using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniPunchAnimationEvents : MonoBehaviour 
{
	[SerializeField] private float screenShakeLength = 1f;
	[SerializeField] private float screenShakeIntensity = 1f;
	[SerializeField] private float screenShakeFrequency = 0.1f;

	private Camera mainCam;
	[SerializeField] private Vector3 intendedCameraPosition;
	[SerializeField] private Vector2 screenShakePositionModifier;
	private float screenShakeTimer;
	private float screenShakeInterValTimer;
	private float currentScreenShakeIntensity;
	private float currentMultiplier;

	private void Start()
	{
		mainCam = Camera.main;
	}

	private void LateUpdate()
	{
		updateCircularScreenShake();
	}
	
	public void cameraShake(float multiplier = 1f)
	{
		currentMultiplier = multiplier;
		screenShakePositionModifier = Vector2.zero;
		screenShakeTimer = screenShakeLength * currentMultiplier;
		currentScreenShakeIntensity = screenShakeIntensity * currentMultiplier;
	}

	void updateCircularScreenShake()
	{
		intendedCameraPosition = mainCam.transform.position - (Vector3)screenShakePositionModifier;
		if (screenShakeTimer > 0f)
			screenShakeTimer -= Time.deltaTime;
		else
		{
			screenShakeTimer = 0f;
			screenShakePositionModifier = Vector2.zero;
		} 
		
		if (screenShakeInterValTimer > 0f)
		{
			screenShakeInterValTimer -= Time.deltaTime;
			screenShakePositionModifier = Vector2.Lerp(screenShakePositionModifier, Vector2.zero, Time.deltaTime / screenShakeInterValTimer);
		}
		else
		{
			screenShakeInterValTimer = screenShakeFrequency;
			screenShakePositionModifier = (Random.insideUnitCircle.normalized * currentScreenShakeIntensity);
			currentScreenShakeIntensity = Mathf.Lerp(currentScreenShakeIntensity, 0f, Mathf.InverseLerp(screenShakeLength * currentMultiplier, 0f, screenShakeTimer));
		}
		mainCam.transform.position = intendedCameraPosition + (Vector3)screenShakePositionModifier;
	}
}
