using UnityEngine;
using System.Collections;

public class VictoryColor : MonoBehaviour
{
	//Changes the color of the object depending on whether the microgame is won or lost or undecided
	//works on both sprites and camera backgrounds

	public Color normalColor, victoryColor, failureColor;

	private SpriteRenderer _spriteRenderer;
	private Camera _camera;

	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_camera = GetComponent<Camera>();
	}
	
	void Update ()
	{
		if (MicrogameController.instance.getVictoryDetermined())
		{
			if (MicrogameController.instance.getVictoryDetermined())
				setColor(victoryColor);
			else
				setColor(failureColor);
		}
		else
			setColor(normalColor);
	}

	void setColor(Color color)
	{
		if (_spriteRenderer != null)
		{
			_spriteRenderer.color = color;
		}
		if (_camera != null)
		{
			_camera.backgroundColor = color;
		}
	}
}
