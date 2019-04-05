using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class HinaGrab: MonoBehaviour
{
	public bool centerOnCursor, disableOnVictory, disableOnLoss;
    public LayerMask layerMask = Physics2D.DefaultRaycastLayers;
    public UnityEvent onGrab, onRelease;
	public Collider2D _collider2D;

	private Vector2 grabOffset;
	private Renderer _renderer;
	private bool _grabbed = false;

	public bool grabbed
	{
		get { return _grabbed; }
		set
		{
			_grabbed = value;
			if (_grabbed)
			{
				grabOffset = (Vector2)transform.position - HinaCam.GCP();
                onGrab.Invoke();
			}
			else
				onRelease.Invoke();
		}
	}

	void Awake ()
    {
        if (_collider2D == null)
            _collider2D = GetComponent<Collider2D>();
		_renderer = GetComponentInChildren<Renderer>();
	}
	
	void Update ()
	{
		if (MicrogameController.instance != null && MicrogameController.instance.getVictoryDetermined())
		{
			if ((disableOnVictory && MicrogameController.instance.getVictory()) || (disableOnLoss && !MicrogameController.instance.getVictory()))
			{
				enabled = false;
				return;
			}
		}

		if (_collider2D == null)
			return;

		updateStatus();

		if (grabbed)
			updateGrab();
	}

	void updateGrab()
	{
		Vector2 position = HinaCam.GCP();
		if (!centerOnCursor)
			position += (Vector2)grabOffset;
		transform.position = position;
	}

	void updateStatus()
	{
		if (!grabbed)
		{
			if (Input.GetMouseButtonDown(0) && HinaCam.isMouseOver(_collider2D, float.PositiveInfinity, layerMask))
				grabbed = true;
		}
		else
		{
			if (!Input.GetMouseButton(0))
				grabbed = false;
		}
		
	}

	public Renderer getRenderer()
	{
		return _renderer;
	}
}
