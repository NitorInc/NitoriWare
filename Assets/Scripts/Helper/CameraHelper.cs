using UnityEngine;
using System.Collections;

public class CameraHelper
{
	public static float camSize;
	public static Vector3 lastCursorPosition;

	/// <summary>
	/// Determines if a certain point in space is not on camera
	/// </summary>
	/// <param name="point"></param>
	/// <returns></returns>
	public static bool isPointOffscreen(Vector3 point)
	{
		camSize = Camera.main.orthographicSize;

		return Mathf.Abs(point.x - Camera.main.transform.position.x) > (camSize * 4f / 3f)
			|| Mathf.Abs(point.y - Camera.main.transform.position.y) > (camSize);
	}

	/// <summary>
	/// Determines if an object is onscreen based on its collider, forceSize will override the collider size
	/// </summary>
	/// <param name="trans"></param>
	/// <param name="forceSize"></param>
	/// <returns></returns>
	public static bool isObjectOffscreen(Transform trans, float forceSize)
	{
		camSize = Camera.main.orthographicSize;

		float size = 0f;
		if (forceSize > 0f)
		{
			size = forceSize;
		}
		else if (trans.GetComponent<BoxCollider2D>() != null)
		{
			BoxCollider2D collider = trans.GetComponent<BoxCollider2D>();
			size = collider.size.x;
			if (collider.size.y > trans.GetComponent<BoxCollider2D>().size.x)
				size = collider.size.y;
			size *= trans.lossyScale.x;
		}
		else if (trans.GetComponent<CircleCollider2D>() != null)
		{
			CircleCollider2D collider = trans.GetComponent<CircleCollider2D>();
			size = collider.radius;
			size *= trans.lossyScale.x;
		}

		return Mathf.Abs(trans.position.x - Camera.main.transform.position.x) > (camSize * 4f / 3f) + size
			|| Mathf.Abs(trans.position.y - Camera.main.transform.position.y) > (camSize) + size;

		//return (trans.position.x > (camSize * 16f / 9f) + size
		//	|| trans.position.x < (camSize * -16f / 9) - size
		//	|| trans.position.y > (camSize) + size
		//	|| trans.position.y < (camSize  * -1f) - size);
	}

	/// <summary>
	/// Determines the position of the player's cursor in world coordinates, useful for making an object move with the mouse
	/// </summary>
	/// <returns></returns>
	public static Vector3 getCursorPosition()
	{
		camSize = Camera.main.orthographicSize;

		Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//if (isPointOffscreen(position))
		//	return lastCursorPosition;

		if (position.x > Camera.main.transform.position.x + (camSize * 4f / 3f))
		{
			position.x = Camera.main.transform.position.x + (camSize * 4f / 3f);
		}
		else if (position.x < Camera.main.transform.position.x - (camSize * 4f / 3f))
		{
			position.x = Camera.main.transform.position.x - (camSize * 4f / 3f);
		}
		if (position.y > Camera.main.transform.position.x + camSize)
		{
			position.y = Camera.main.transform.position.x + camSize;
		}
		else if (position.y < Camera.main.transform.position.x - camSize)
		{
			position.y = Camera.main.transform.position.x - camSize;
		}

		lastCursorPosition = position;
		return position;
	}

	/// <summary>
	/// Determines the position of the player's cursor in world coordinates regardless of whether it's offscreen
	/// </summary>
	/// <returns></returns>
	public static Vector3 getCursorPositionAbsolute()
	{
		camSize = Camera.main.orthographicSize;

		Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return position;
	}

	/// <summary>
	/// Determines wheteher the mouse is hovering over a particular collider, useful for making an object clickable
	/// </summary>
	/// <param name="collider"></param>
	/// <returns></returns>
	public static bool isMouseOver(Collider2D collider)
	{

		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity);
		return (hit.collider == collider);
	}
}
