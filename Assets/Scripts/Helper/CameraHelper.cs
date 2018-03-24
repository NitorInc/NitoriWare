using UnityEngine;
using System.Collections.Generic;

public class CameraHelper
{
	public static float camSize;
	public static Vector3 lastCursorPosition;

    private static CameraCollisionResults cameraCollisionResults = new CameraCollisionResults(-1);
    private struct CameraCollisionResults
    {
        public int evaluatedFrame;
        public Dictionary<int, RaycastHit2D> calculatedHits;

        public CameraCollisionResults(int evaluatedFrame)
        {
            this.evaluatedFrame = evaluatedFrame;
            calculatedHits = new Dictionary<int, RaycastHit2D>();
        }
    }

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
	/// Determines if an object is offscreen based on its collider
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="forceSize"></param>
	/// <returns></returns>
	public static bool isObjectOffscreen(Transform transform)
	{
		float size;
		if (transform.GetComponent<BoxCollider2D>() != null)
		{
			BoxCollider2D collider = transform.GetComponent<BoxCollider2D>();
			size = collider.size.x;
			if (collider.size.y > transform.GetComponent<BoxCollider2D>().size.x)
				size = collider.size.y;
			size *= transform.lossyScale.x;
		}
		else if (transform.GetComponent<CircleCollider2D>() != null)
		{
			CircleCollider2D collider = transform.GetComponent<CircleCollider2D>();
			size = collider.radius;
			size *= transform.lossyScale.x;
		}
		else
			size = 0f;
		return isObjectOffscreen(transform, size);
	}

	/// <summary>
	/// Determines if an object is offscreen, must be at least forceSize distance away from edge of screen
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="forceSize"></param>
	/// <returns></returns>
	public static bool isObjectOffscreen(Transform transform, float forceSize)
	{
		camSize = Camera.main.orthographicSize;
		return Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > (camSize * 4f / 3f) + forceSize
			|| Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > (camSize) + forceSize;
	}

	
	/// <summary>
	/// Determines the position of the player's cursor in world coordinates, useful for making an object move with the mouse
	/// </summary>
	/// <returns></returns>
	public static Vector3 getCursorPosition()
	{
		return getCursorPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).z);
	}

	/// <summary>
	/// Determines the position of the player's cursor in world coordinates with z value specified, useful for making an object move with the mouse
	/// </summary>
	/// <returns></returns>
	public static Vector3 getCursorPosition(float z)
	{
		if (Camera.main.orthographic)
		{
			camSize = Camera.main.orthographicSize;

			Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//if (isPointOffscreen(position))
			//	return lastCursorPosition;

			position.x = Mathf.Clamp(position.x, Camera.main.transform.position.x - (camSize * 4f / 3f),
				Camera.main.transform.position.x + (camSize * 4f / 3f));
			position.y = Mathf.Clamp(position.y, Camera.main.transform.position.y - camSize,
				Camera.main.transform.position.y + camSize);
			position.z = z;

			lastCursorPosition = position;
			return position;
		}
		else
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
			float distance;
			xy.Raycast(mouseRay, out distance);
			return mouseRay.GetPoint(distance);
		}
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
	public static bool isMouseOver(Collider2D collider, float distance = float.PositiveInfinity, int layerMask = Physics2D.DefaultRaycastLayers)
	{
        if (cameraCollisionResults.evaluatedFrame != Time.frameCount)
            cameraCollisionResults = new CameraCollisionResults(Time.frameCount);
        else if (cameraCollisionResults.calculatedHits.ContainsKey(layerMask))
            return (cameraCollisionResults.calculatedHits[layerMask].collider == collider);
        
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, distance, layerMask);
        cameraCollisionResults.calculatedHits[layerMask] = hit;
        return (hit.collider == collider);
    }
}
