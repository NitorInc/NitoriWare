using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MouseGrabbable : MonoBehaviour
{
	public bool centerOnCursor, disableOnVictory, disableOnLoss;
	public UnityEvent onGrab, onRelease;

	private Collider2D _collider2D;
	private Vector2 grabOffset;
	private SpriteRenderer _spriteRenderer;
	private bool _grabbed = false;

	public bool grabbed
	{
		get { return _grabbed; }
		set
		{
			_grabbed = value;
			if (_grabbed)
			{
				grabOffset = (Vector2)transform.position - (Vector2)CameraHelper.getCursorPosition();
				onGrab.Invoke();
			}
			else
				onRelease.Invoke();
		}
	}

	void Awake ()
	{
		_collider2D = GetComponent<Collider2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
	{
		if (MicrogameController.instance.getVictoryDetermined())
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
		Vector3 position = CameraHelper.getCursorPosition();
		if (!centerOnCursor)
			position += (Vector3)grabOffset;
		position.z = transform.position.z;
		transform.position = position;
	}

	void updateStatus()
	{
		if (!grabbed)
		{
			if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(_collider2D))
				grabbed = true;
		}
		else
		{
			if (!Input.GetMouseButton(0))
				grabbed = false;
		}
		
	}

	public SpriteRenderer getSpriteRenderer()
	{
		return _spriteRenderer;
	}
}
